using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GrannyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject gameOverPanel;

    [Header("Behavior Settings")]
    public Transform[] wanderPoints;
    public float wanderSpeed = 2f;
    public float chaseSpeed = 5f;
    public float attackDistance = 2f;
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public float memoryDuration = 4f;
    public float investigateTimeout = 6f;
    public float soundReactionRadius = 20f;

    private int currentWanderIndex = 0;
    private Vector3 investigateTarget;
    private float investigateTimer = 0f;
    private float lastSeenTime = -999f;
    private Vector3 lastKnownPlayerPos;

    private enum State { Wander, Investigate, Chase, Attack }
    private State state = State.Wander;
    private bool isAttacking = false;
    private bool playerCaught = false;

    void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (wanderPoints == null || wanderPoints.Length == 0)
        {
            wanderPoints = GameObject.FindGameObjectsWithTag("Waypoint")
                            .Select(go => go.transform).ToArray();
        }

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        animator.applyRootMotion = false;

        agent.speed = wanderSpeed;
        agent.stoppingDistance = 0.5f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.isStopped = false;
    }

    void Start()
    {
        if (wanderPoints.Length > 0 && agent.isOnNavMesh)
            GoToNextWanderPoint();
    }

    void Update()
    {
        if (playerCaught || agent == null || !agent.isOnNavMesh)
            return;

        // Check if player visible
        if (PlayerInSight())
        {
            lastSeenTime = Time.time;
            lastKnownPlayerPos = player.position;

            float distance = Vector3.Distance(transform.position, player.position);

            // ✅ if already close enough, attack immediately
            if (distance <= attackDistance + 0.3f)
            {
                if (state != State.Attack && !isAttacking)
                {
                    ChangeState(State.Attack);
                    StartCoroutine(AttackPlayer());
                    return;
                }
            }
            else
            {
                // otherwise chase
                if (state != State.Chase)
                    ChangeState(State.Chase);
            }
        }


        switch (state)
        {
            case State.Wander:
                DoWander();
                break;

            case State.Investigate:
                DoInvestigate();
                break;

            case State.Chase:
                DoChase();
                break;

            case State.Attack:
                // attack coroutine handles itself
                break;
        }
    }

    // --- STATE HANDLERS ---

    void DoWander()
    {
        agent.isStopped = false;
        agent.speed = wanderSpeed;
        animator?.Play(agent.velocity.magnitude > 0.1f ? "Walk" : "Idle");

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWanderPoint();
    }

    void DoInvestigate()
    {
        agent.isStopped = false;
        agent.speed = wanderSpeed;
        if (agent.destination != investigateTarget)
            agent.SetDestination(investigateTarget);

        animator?.Play(agent.velocity.magnitude > 0.1f ? "Walk" : "Idle");

        if (!agent.pathPending && agent.remainingDistance < 0.6f)
        {
            investigateTimer += Time.deltaTime;
            if (investigateTimer >= investigateTimeout)
                ChangeState(State.Wander);
        }
    }

    void DoChase()
    {
        if (player == null) return;

        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = attackDistance;

        // Update chase destination constantly
        if (agent.isOnNavMesh)
            agent.SetDestination(player.position);

        animator?.Play(agent.velocity.magnitude > 0.1f ? "Run" : "Idle");

        float dist = Vector3.Distance(transform.position, player.position);

        // ✅ Direct attack if close enough
        if (dist <= attackDistance + 0.2f && PlayerInSight())
        {
            if (!isAttacking)
            {
                agent.isStopped = true; // stop to swing
                ChangeState(State.Attack);
                StartCoroutine(AttackPlayer());
                return;
            }
        }

        // ✅ If lost sight for too long, investigate
        if (Time.time - lastSeenTime > memoryDuration)
        {
            investigateTarget = lastKnownPlayerPos;
            investigateTimer = 0f;
            ChangeState(State.Investigate);
        }
    }


    // --- HELPER METHODS ---

    private void ChangeState(State newState)
    {
        if (state == newState) return;

        isAttacking = false;
        investigateTimer = 0f;

        if (newState == State.Wander || newState == State.Investigate)
            agent.stoppingDistance = 0.5f;
        else if (newState == State.Chase)
            agent.stoppingDistance = attackDistance;

        if (newState == State.Attack)
            agent.isStopped = true;
        else
            agent.isStopped = false;

        state = newState;
    }

    bool PlayerInSight()
    {
        if (player == null) return false;

        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 dir = (player.position + Vector3.up - origin).normalized;
        float dist = Vector3.Distance(origin, player.position + Vector3.up);
        float angle = Vector3.Angle(transform.forward, dir);

        if (angle < viewAngle / 2f && dist < viewRadius)
        {
            if (Physics.Raycast(origin, dir, out RaycastHit hit, dist, ~0, QueryTriggerInteraction.Ignore))
            {
                UnityEngine.Debug.DrawRay(origin, dir * dist, Color.red);
                if (hit.collider != null && (hit.collider.transform == player || hit.collider.transform.IsChildOf(player)))
                    return true;
            }
        }
        return false;
    }

    public void HearSound(Vector3 soundPosition)
    {
        if (Vector3.Distance(transform.position, soundPosition) > soundReactionRadius)
            return;

        if (state == State.Chase || state == State.Attack)
            return;

        investigateTarget = soundPosition;
        investigateTimer = 0f;
        ChangeState(State.Investigate);
        agent.SetDestination(investigateTarget);
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        // play attack anim if any (even empty)
        if (animator != null)
            animator.Play("Attack");

        // wait a bit to simulate attack duration
        yield return new WaitForSeconds(0.8f);

        float dist = Vector3.Distance(transform.position, player.position);
        if (PlayerInSight() && dist <= attackDistance + 0.5f)
        {
            PlayerCaught();  // ✅ ensures player dies
        }
        else
        {
            // if missed, resume chase
            ChangeState(State.Chase);
        }

        isAttacking = false;
    }


    void PlayerCaught()
    {
        playerCaught = true;
        agent.isStopped = true;
        animator?.Play("Attack");

        GameOverManager.Instance.TriggerGameOver(); // unified call
    }

    void GoToNextWanderPoint()
    {
        if (wanderPoints.Length == 0) return;
        currentWanderIndex = (currentWanderIndex + 1) % wanderPoints.Length;
        agent.SetDestination(wanderPoints[currentWanderIndex].position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, soundReactionRadius);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GrannyProximityKill : MonoBehaviour
{
    [Header("Kill settings")]
    public float killRange = 1.5f;
    public string playerTag = "Player";
    public bool requireLineOfSight = false; // if true, will raycast before killing
    public LayerMask lineOfSightMask = ~0;

    [Header("Optional UI")]
    public GameObject gameOverPanel;

    bool playerDead = false;
    Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (playerDead || player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= killRange)
        {
            if (requireLineOfSight)
            {
                Vector3 origin = transform.position + Vector3.up * 1.2f;
                Vector3 dir = (player.position + Vector3.up - origin).normalized;
                float d = Vector3.Distance(origin, player.position + Vector3.up);
                if (Physics.Raycast(origin, dir, out RaycastHit hit, d, lineOfSightMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider != null && (hit.collider.transform == player || hit.collider.transform.IsChildOf(player)))
                    {
                        KillPlayer();
                    }
                }
            }
            else
            {
                KillPlayer();
            }
        }
    }

   
        void KillPlayer()
        {
            playerDead = true;
            GameOverManager.Instance.TriggerGameOver(); // unified call
        }
    

    // draw range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, killRange);
    }
}

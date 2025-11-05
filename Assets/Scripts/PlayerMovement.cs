using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float groundCheckRadius = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 slopeNormal;
    private float playerHeight;

    [Header("Input Settings")]
    [SerializeField] private string horizontalInput = "Horizontal";
    [SerializeField] private string verticalInput = "Vertical";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Calculate player height automatically
        Collider col = GetComponent<Collider>();
        if (col != null)
            playerHeight = col.bounds.size.y;
        else
            playerHeight = transform.localScale.y * 2f;
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        StickToGround();
    }

    private void MovePlayer()
    {
        float x = Input.GetAxis(horizontalInput);
        float z = Input.GetAxis(verticalInput);

        Vector3 moveDir = transform.right * x + transform.forward * z;

        if (isGrounded)
            moveDir = Vector3.ProjectOnPlane(moveDir, slopeNormal); // follow slope

        Vector3 moveVelocity = moveDir.normalized * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    // Force player to stay on ground (prevents floating)
    private void StickToGround()
    {
        if (isGrounded)
        {
            rb.position = new Vector3(rb.position.x, rb.position.y - 0.01f, rb.position.z); // small downward force
        }
    }

    private void CheckGround()
    {
        Vector3 checkPos = transform.position + Vector3.down * (playerHeight / 2 + 0.1f);
        isGrounded = Physics.CheckSphere(checkPos, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight, groundLayer))
            {
                slopeNormal = hit.normal;
            }
        }
        else
        {
            slopeNormal = Vector3.up;
        }
    }

    private void OnDrawGizmosSelected()
    {
        float height = 2f;
        Collider col = GetComponent<Collider>();
        if (col != null)
            height = col.bounds.size.y;
        else
            height = transform.localScale.y * 2f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * (height / 2 + 0.1f), groundCheckRadius);
    }
}

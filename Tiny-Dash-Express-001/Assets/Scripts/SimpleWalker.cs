using UnityEngine;

public class SimpleWalker : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float changeDirectionTime = 8f;
    public float boundarySize = 30f;

    [Header("Physics Settings")]
    public float pushForce = 8f;

    private Vector3 startPosition;
    private float timer;
    private Rigidbody rb;
    private bool isKnocked = false;
    private float knockTimer = 0f;
    private float stuckTimer = 0f;
    private Vector3 lastPosition;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        // Configure Rigidbody
        rb.mass = 50f;
        rb.linearDamping = 2f;
        rb.angularDamping = 3f;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                        RigidbodyConstraints.FreezeRotationZ |
                        RigidbodyConstraints.FreezePositionY;

        PickNewDirection();
        lastPosition = transform.position;
    }

    void Update()
    {
        // If knocked, wait before moving again
        if (isKnocked)
        {
            knockTimer -= Time.deltaTime;
            if (knockTimer <= 0)
            {
                isKnocked = false;
                rb.linearVelocity = Vector3.zero;
                PickNewDirection(); // Pick new direction after being knocked
            }
            return;
        }

        // Check if stuck (not moving for 2 seconds)
        if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 2f)
            {
                // Stuck! Turn to a completely new direction
                Debug.Log("Cow got stuck - turning around");
                PickNewDirection();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f; // Reset stuck timer if moving
        }

        lastPosition = transform.position;

        // Move forward
        rb.linearVelocity = transform.forward * walkSpeed;

        // Check boundary
        float distanceFromStart = Vector3.Distance(transform.position, startPosition);
        if (distanceFromStart > boundarySize)
        {
            TurnAround();
        }

        // Timer to change direction
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            PickNewDirection();
        }
    }

    void PickNewDirection()
    {
        // Pick random angle (0 to 360)
        float randomAngle = Random.Range(0f, 360f);

        // Create new rotation
        Quaternion newRotation = Quaternion.Euler(0, randomAngle, 0);

        // Apply rotation instantly
        transform.rotation = newRotation;

        // Reset timer with slight random variation
        timer = changeDirectionTime + Random.Range(-1f, 1f);

        // Clear velocity to prevent sliding
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        Debug.Log($"Cow changed direction to angle: {randomAngle}");
    }

    void TurnAround()
    {
        // Simple 180 degree turn
        float currentY = transform.eulerAngles.y;
        float newY = currentY + 180f;

        transform.rotation = Quaternion.Euler(0, newY, 0);
        timer = changeDirectionTime;

        Debug.Log("Cow hit boundary - turned around");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Truck"))
        {
            // Knockback
            Vector3 knockDirection = (transform.position - collision.transform.position).normalized;
            knockDirection.y = 0.2f;

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(knockDirection * pushForce, ForceMode.Impulse);

            isKnocked = true;
            knockTimer = 1.5f;

            // Force a new direction after being knocked
            Invoke("PickNewDirection", 1.6f);
        }

        // If cow hits a wall or obstacle, turn around immediately
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Obstacle"))
        {
            TurnAround();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(startPosition, boundarySize);
        }
    }
}
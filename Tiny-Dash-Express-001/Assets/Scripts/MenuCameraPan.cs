using UnityEngine;

public class MenuCameraPan : MonoBehaviour
{
    [Header("Camera Movement")]
    public Vector3 startPosition;    // Where camera begins
    public Vector3 endPosition;      // Where camera ends
    public float panDuration = 10f;  // How long to take (seconds)

    [Header("Camera Rotation")]
    public Vector3 fixedRotation = new Vector3(15f, 0f, 0f);  // Fixed 15бу down angle

    [Header("Optional Settings")]
    public bool loopPan = false;      // Restart when done?
    public float loopDelay = 2f;      // Wait before restarting
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float timer = 0f;
    private bool isPanning = true;
    private float waitTimer = 0f;

    void Start()
    {
        // Set camera to start position
        transform.position = startPosition;

        // Apply fixed rotation (X=15, Y=0, Z=0)
        transform.eulerAngles = fixedRotation;
    }

    void Update()
    {
        if (!isPanning) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / panDuration);

        // Apply curve for smooth acceleration/deceleration
        float curvedT = movementCurve.Evaluate(t);

        // Move camera only (rotation stays fixed)
        transform.position = Vector3.Lerp(startPosition, endPosition, curvedT);

        // When pan completes
        if (t >= 1f)
        {
            if (loopPan)
            {
                // Reset with delay
                isPanning = false;
                waitTimer = loopDelay;
            }
            else
            {
                enabled = false; // Stop script
                Debug.Log("Camera pan complete!");
            }
        }
    }

    void LateUpdate()
    {
        if (!isPanning && loopPan)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                // Reset and start again
                timer = 0f;
                transform.position = startPosition;
                transform.eulerAngles = fixedRotation;  // Reset rotation too
                isPanning = true;
            }
        }
    }

    // Optional: Draw path in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, 1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}
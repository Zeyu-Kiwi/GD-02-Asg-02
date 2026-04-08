using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    public float floatSpeed = 1f;      // How fast it bobs
    public float floatHeight = 0.3f;   // How high it moves up/down

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void LateUpdate()
    {
        // 1. Always face the camera (billboard effect)
        transform.LookAt(Camera.main.transform);

        // If your question mark still faces backward, uncomment the next line:
        // transform.Rotate(0, 180, 0);

        // 2. Gentle floating up and down
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
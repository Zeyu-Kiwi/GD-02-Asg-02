using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Transform mark;
    public Transform zoneMark;

    [Header("Rotation")]
    public float rotationSpeed = 100f;
    [Header("Scaling")]
    public float scaleSpeed = 2f;
    public float scaleAmount = 0.2f;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = zoneMark.transform.localScale;
    }

    private void Update()
    {
        Rotate();
        ScalePulse();
    }

    private void LateUpdate()
    {
        MiniMapCamFollow();
        ZoneMarkAttach();
    }

    void Rotate()
    {
        zoneMark.transform.Rotate(0f, 0f, rotationSpeed * Time.unscaledDeltaTime);
    }

    void ScalePulse()
    {
        float scale = 1 + Mathf.Sin(Time.unscaledTime * scaleSpeed) * scaleAmount;
        zoneMark.transform.localScale = initialScale * scale;
    }

    private void MiniMapCamFollow()
    {
        if (mark == null)
        {
            return;
        }

        Vector3 newposition = player.position;
        newposition.y = transform.position.y;
        transform.position = newposition;

        newposition.y = mark.transform.position.y;
        mark.transform.position = newposition;

        //rotate cam with truck
        //transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        mark.transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y - 90f, 0f);
    }

    private void ZoneMarkAttach()
    {
        if (zoneMark == null)
        {
            return;
        }

        Vector3 newposition = player.position;
        newposition.y = transform.position.y;
        transform.position = newposition;

        newposition.y = zoneMark.transform.position.y;
        zoneMark.transform.position = newposition;
    }
}

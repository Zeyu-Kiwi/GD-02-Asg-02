using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Transform mark;
    public Transform zoneMark;

    private void LateUpdate()
    {
        MiniMapCamFollow();
        ZoneMarkAttach();
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

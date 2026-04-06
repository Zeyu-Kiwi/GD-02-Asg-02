using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Transform mark;

    private void LateUpdate()
    {
        MiniMapCamFollow();
    }

    private void MiniMapCamFollow()
    {
        Vector3 newposition = player.position;
        newposition.y = transform.position.y;
        transform.position = newposition;

        newposition.y = mark.transform.position.y;
        mark.transform.position = newposition;



        //rotate cam with truck
        //transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        mark.transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y - 90f, 0f);
    }
}

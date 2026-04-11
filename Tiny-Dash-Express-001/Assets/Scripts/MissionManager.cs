using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public GameObject[] startZones;
    public GameObject[] endZones;

    public int missionIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int s = 0; s < startZones.Length; s++) //loop through all start zones and only active the right one
        {
            if (s == missionIndex)
            {
                startZones[s].SetActive(true);
            }
            else
            {
                startZones[s].SetActive(false);
            }
        }

        for (int e = 0; e < endZones.Length; e++) //loop through all end zones and only active the right one
        {
            if (e == missionIndex)
            {
               endZones[e].SetActive(true);
            }
            else
            {
                endZones[e].SetActive(false);
            }
        }
    }
}

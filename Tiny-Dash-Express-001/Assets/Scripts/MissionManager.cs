using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public GameObject[] startZones;
    public GameObject[] endZones;
    public GameObject[] startMarkers;
    public GameObject[] endMarkers;

    public GameObject endScreen;
    [SerializeField] private bool isGameEnd = false;

    public int missionIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        isGameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetMissionStates();
        GameCompleted();
    }

    void SetMissionStates()
    {
        for (int s = 0; s < startZones.Length; s++) //loop through all start zones and only active the right one
        {
            if (s == missionIndex)
            {
                startZones[s].SetActive(true);
                startMarkers[s].SetActive(true);
            }
            else
            {
                startZones[s].SetActive(false);
                startMarkers[s].SetActive(false);
            }
        }

        for (int e = 0; e < endZones.Length; e++) //loop through all end zones and only active the right one
        {
            if (e == missionIndex)
            {
                endZones[e].SetActive(true);
                endMarkers[e].SetActive(true);
            }
            else
            {
                endZones[e].SetActive(false);
                endMarkers[e].SetActive(false);
            }
        }
    }

    void GameCompleted()
    {
        if (missionIndex == 4 && !isGameEnd)
        {
            isGameEnd = true;
            GameManager.Instance.timeTaken = GameManager.Instance.elapsedTime;
            GameManager.Instance.BestTimeUpdate();

            //Debug.Log("all deliveryed!");
            AudioManager.instance.Stop("Theme");
            AudioManager.instance.Play("GameComplete");

            endScreen.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

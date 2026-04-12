using System.Dynamic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalMoney = 0;

    public TextMeshProUGUI timerText;
    public float elapsedTime;
    public float timeTaken;

    public TextMeshProUGUI timeTakenTxt;
    public TextMeshProUGUI bestTimeTxt;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FixErrorSave();
        SetMouseState();
    }

    // Update is called once per frame
    void Update()
    {
        MyTimer();
    }

    [ContextMenu("Reset Best Time Saved")]
    public void ResetSavedTime()
    {
        PlayerPrefs.DeleteKey("BestTime");
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        //Debug.Log("Total money: " + totalMoney);
    }

    private void MyTimer()
    {
        elapsedTime += Time.unscaledDeltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void BestTimeUpdate()
    {
        //is there already a besttime?
        if (PlayerPrefs.HasKey("BestTime"))
        {
            //Debug.Log("Save found, comparing time");
            //if the new time is shorter than the saved one?
            if (timeTaken < PlayerPrefs.GetFloat("BestTime"))
            {
                //set a new best time
                PlayerPrefs.SetFloat("BestTime", timeTaken);
            }   
        }
        else
        {
            //Debug.Log("No save, saving current time");
            PlayerPrefs.SetFloat("BestTime", timeTaken);
        }
        float tempBT = PlayerPrefs.GetFloat("BestTime");

        //update TMPro
        int minutesTT = Mathf.FloorToInt(timeTaken / 60);
        int secondsTT = Mathf.FloorToInt(timeTaken % 60);
        timeTakenTxt.text = string.Format("{0:00}:{1:00}", minutesTT, secondsTT);

        //Debug.Log(PlayerPrefs.GetFloat("BestTime"));
        int minutesBT = Mathf.FloorToInt(tempBT / 60);
        int secondsBT = Mathf.FloorToInt(tempBT % 60);
        bestTimeTxt.text = string.Format("{0:00}:{1:00}", minutesBT, secondsBT);


    }

    void FixErrorSave()
    {
        if (PlayerPrefs.HasKey("BestTime"))
        {
            //Debug.Log("Found Save");
            float checkSave = PlayerPrefs.GetFloat("BestTime");
            if (checkSave <= 5f)
            {
                //Debug.Log("Error data. Removing save");
                ResetSavedTime();
            }
        }
    }

    void SetMouseState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

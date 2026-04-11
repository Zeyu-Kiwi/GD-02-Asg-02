using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    public float waitTime = 10f;

    public GameObject roadBlocks;

    private void Update()
    {
        CheckInput();
        

        //for (int i = 0; i < popUps.Length; i++)
        //{
        //    if (i == popUpIndex)
        //    {
        //        //Debug.Log("current index: " + i + ", active tutorial");
        //        popUps[i].SetActive(true);
        //    }
        //    else
        //    {
        //        //Debug.Log("current index: " + i + ", deactive tutorial");
        //        popUps[i].SetActive(false);
        //    }
        //}

        
    }

    void ShowPopup(int index)
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            popUps[i].SetActive(i == index);
        }
    }

    void CheckInput()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        if (popUpIndex == 0) //tutorial start: learn to use WASD
        {
            if (Mathf.Abs(moveInput) > 0.1f || Mathf.Abs(steerInput) > 0.1f)
            {
                //Debug.Log("learnt to move");
                popUpIndex++;
                ShowPopup(popUpIndex);
            }
        }
        else if (popUpIndex == 1) // tutorial: learn to brake
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("learnt to brake");
                popUpIndex++;
                ShowPopup(popUpIndex);
            }
        }
        else if (popUpIndex == 2) // tutorial: learn to drift
        {
            if (Mathf.Abs(steerInput) > 0.1f && Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("learnt to drift");
                popUpIndex++;
                ShowPopup(popUpIndex);
            }
        }
        else if (popUpIndex == 3) // tutorial learn to reload
        {
            if (SaveSystem.Instance.isLoaded && Input.GetKeyDown(KeyCode.R)) // move to next tutorial if player perform a reload
            {
                //Debug.Log("learnt to reload");
                roadBlocks.SetActive(false);
                popUpIndex++;
                ShowPopup(popUpIndex);
                waitTime = 7f;
            }

            if (waitTime <= 0 && roadBlocks.activeInHierarchy) // move to next tutorial if times up and roadblock is still active
            {
                roadBlocks.SetActive(false);
                popUpIndex++;
                ShowPopup(popUpIndex);
                waitTime = 7f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        else if (popUpIndex == 4) // tutorial end txt
        {
            
            if (waitTime <= 0)
            {
                //Debug.Log("end txt");
                popUpIndex++;
                ShowPopup(popUpIndex);
            }
            else{
                waitTime -= Time.deltaTime;
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbCountDown : MonoBehaviour
{
    [SerializeField] float timeRemaining = 30;
    [SerializeField] bool timerIsRunning = false;
    public Text timeText;
    public GameObject myPrefab;
    bool spawned = false;

    private void Start()
    {
        
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                spawned = true;
                Instantiate(myPrefab, new Vector3(-7.67f, 2.02f, 0f), Quaternion.identity);
                timeRemaining = 0;
                timerIsRunning = false;
            }
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if(!spawned)
        {
            timeText.text = string.Format("Orb Will Spawn In" + "{0:00}:{1:00}", minutes, seconds);
        }
        else 
        {
            spawned = false;
            timeText.text = string.Format(" ");
        }
    }

    public void TimeStart()
    {
        timeRemaining = 15f;
        timerIsRunning = true;
    }
}

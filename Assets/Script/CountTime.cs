using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountTime : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timerIsRunning = false;
    public Text timeText;

    [Header("Win")]
    public GameObject win;
    IEnumerator coroutine;

    [Header("Player")]
    Transform Player;
    PlayerAttacks Pstats;
    int playerHealth;

    [Header("Enemy")]
    Transform Enemy;
    EnemyBoss EnemyStats;
    int EnemyHealth;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        checkplayer();
        Pstats = Player.GetComponent<PlayerAttacks>();
        checkenemy();
        EnemyStats = Enemy.GetComponent<EnemyBoss>();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                win.SetActive(false);
            }
            else
            {
                //Debug.Log("Time has run out!");
                playerHealth = Pstats.GetHealthStats();
                EnemyHealth = EnemyStats.GetHealthStats();
                if(playerHealth > EnemyHealth)
                {
                    win.SetActive(true);
                    timeRemaining = 0;
                    timerIsRunning = false;
                    coroutine = Wait(1f);
                    StartCoroutine(coroutine);
                }
                else
                {
                    win.SetActive(false);
                    timeRemaining = 0;
                    timerIsRunning = false;
                    SceneManager.LoadScene(6);
                } 
            }
        }
        DisplayTime(timeRemaining);
    }

    void DisplayTime(float timeToDisplay)
    {
    float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
    float seconds = Mathf.FloorToInt(timeToDisplay % 60);

    timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    void checkplayer()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (GameObject.FindGameObjectWithTag("PlayerW") != null)
        {
            Player = GameObject.FindGameObjectWithTag("PlayerW").transform;
        }
    }

    void checkenemy()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            Enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
    }

    private IEnumerator Wait(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        changeScene();
    }
    void changeScene()
    {
        PlayerPrefs.SetInt("Unlock", 4);
        SceneManager.LoadScene(0);
    }
}

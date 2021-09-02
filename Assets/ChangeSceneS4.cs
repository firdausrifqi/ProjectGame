using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneS4 : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator coroutine;
    public GameObject BOSS;
    public GameObject win;
    EnemyBossFSM boss;
    private void Start()
    {
        boss = BOSS.GetComponent<EnemyBossFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.isDead)
        {
            coroutine = Wait(0.8f);
            StartCoroutine(coroutine);
        }
        else win.SetActive(false);
    }
    private IEnumerator Wait(float waitTime)
    {
        win.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        changeScene();
    }
    void changeScene()
    {
        PlayerPrefs.SetInt("WhatStage",4);
        SceneManager.LoadScene(4);
    }
}

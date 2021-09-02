using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changescene : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator coroutine;
    public GameObject BOSS;
    public GameObject win;
    EnemyBoss boss;
    private void Start()
    {
        boss = BOSS.GetComponent<EnemyBoss>();
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
        PlayerPrefs.SetInt("Unlock", 4);
        SceneManager.LoadScene(0);
    }
}

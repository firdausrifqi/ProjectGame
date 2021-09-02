using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    // IEnumerator coroutine;

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "HeadP" || other.gameObject.tag == "HeadE")
    //     {
    //         GetComponent<Collider2D>().enabled = false;
    //         coroutine = Miss(0.7f);
    //         StartCoroutine(coroutine);
    //     }
    // }

    // private IEnumerator Miss(float waitTime)
    // {
    //     yield return new WaitForSeconds(waitTime);
    //     GetComponent<Collider2D>().enabled = true;
    // }

    private PlatformEffector2D effector;
    public float waitTime;
    int count = 0;
    bool waktujalan = false;
    
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();   
    }

    void Update()
    {
        //Debug.Log(waitTime);
        if (waitTime <= 0){
            count = 0;
            waktujalan = false;
        }else{
            waitTime -= Time.deltaTime;
        }
        //batas
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("s"))
        {
            if(!waktujalan)
            {
                waktujalan = true;
                waitTime = 0.5f;
            }
            count++;
        }
        //batas
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown("w") || Input.GetKeyDown("space"))
        {
            tembuskeatas();
        }
        //batas
        if (count == 2){
            tembuskebawah();
            count = 0;
        }
    }

    public void tembuskeatas()
    {
        effector.rotationalOffset = 0;
    }
    public void tembuskebawah()
    {
        effector.rotationalOffset = 180f;
    }


}

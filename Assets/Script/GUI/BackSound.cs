using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Sound On")==null)
        {
            GetComponent<AudioSource>().Play();
            gameObject.name = "Sound On";
            PlayerPrefs.SetFloat("volume", 1);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
    }
}

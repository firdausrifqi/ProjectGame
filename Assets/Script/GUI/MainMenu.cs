using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void exit()
    {
        Application.Quit();
    }

    public void play(string scene_name)
    {
        Application.LoadLevel(scene_name);
    }

    public void sound_volume(float volume)
    {
        PlayerPrefs.SetFloat("volume",volume);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

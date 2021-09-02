using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseChar : MonoBehaviour
{

    int charselect;
    public GameObject Warrior;
    public GameObject LBandit;
    int Unlocked = 0;
    // Start is called before the first frame update
    void Start()
    {
        charselect = PlayerPrefs.GetInt("ChooseChar");

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Unlock") == 4)
        {
            Unlocked = 4;
        }
        else Unlocked = 0;

        if (charselect == 4 && Unlocked == 4)
        {
            Warrior.SetActive(true);
        }
        else LBandit.SetActive(true);
    }
}

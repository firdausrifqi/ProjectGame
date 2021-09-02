using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector_Script1 : MonoBehaviour {

    public GameObject Wood;
    public GameObject Grave;
    public GameObject Steam;
    private Vector3 CharacterPosition;
    private Vector3 OffScreen;
    private int CharacterInt = 1;
    private SpriteRenderer WoodRender, GraveRender, SteamRender;
    private void Awake()
    {
        CharacterPosition = Wood.transform.position;
        OffScreen = Grave.transform.position;
        WoodRender = Wood.GetComponent<SpriteRenderer>();
        GraveRender = Grave.GetComponent<SpriteRenderer>();
        SteamRender = Steam.GetComponent<SpriteRenderer>();
    }

    public void NextCharacter()
    {
        switch (CharacterInt)
        {
            case 1:
                WoodRender.enabled = false;
                Wood.transform.position = OffScreen;
                Steam.transform.position = CharacterPosition;
                SteamRender.enabled = true;
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }
    public void PreviousCharacter()
    {
        switch (CharacterInt)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }
}

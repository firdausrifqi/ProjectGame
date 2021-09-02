using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selector_Script : MonoBehaviour
{
    public GameObject LBandit;
    public GameObject GraveRobber;
    public GameObject SteamMan;
    public GameObject WoodCutter;
    public GameObject Warior;
    public GameObject Knight;

    private Vector3 newpos;

    private Vector3 CharacterPosition;
    private Vector3 OffScreen;

    private int characterInt = 1;

    private SpriteRenderer WoodcutterRender, GraveRobberRender, SteamManRender, WariorRender, LBanditRender, KnightRender;
    private readonly string selectedCharacter = "SelectedCharacter";

    private void Awake()
    {
        CharacterPosition = LBandit.transform.position;
        OffScreen = GraveRobber.transform.position;

        LBanditRender = LBandit.GetComponent<SpriteRenderer>();
        GraveRobberRender = LBandit.GetComponent<SpriteRenderer>();
        SteamManRender = LBandit.GetComponent<SpriteRenderer>();
        WoodcutterRender = LBandit.GetComponent<SpriteRenderer>();
        WariorRender = LBandit.GetComponent<SpriteRenderer>();
        KnightRender = LBandit.GetComponent<SpriteRenderer>();
    }

    public void Next()
    {
        switch (characterInt)
        {
            case 1:
                PlayerPrefs.SetInt(selectedCharacter, 1);
                LBanditRender.enabled = false;
                LBandit.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                Warior.transform.position = newpos;
                WariorRender.enabled = true;
                characterInt++;
                break;
            case 2:
                PlayerPrefs.SetInt(selectedCharacter, 2);
                WariorRender.enabled = false;
                Warior.transform.position = OffScreen;
                //newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                LBandit.transform.position = CharacterPosition;
                LBanditRender.enabled = true;
                characterInt++;
                ResetInt();
                break;
           /* case 3:
                PlayerPrefs.SetInt(selectedCharacter, 3);
                SteamManRender.enabled = false;
                SteamMan.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                WoodCutter.transform.position = newpos;
                WoodcutterRender.enabled = true;
                characterInt++;
                break;
            case 4:
                PlayerPrefs.SetInt(selectedCharacter, 4);
                WoodcutterRender.enabled = false;
                WoodCutter.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                Warior.transform.position = newpos;
                WariorRender.enabled = true;
                characterInt++;
                break;
            case 5:
                PlayerPrefs.SetInt(selectedCharacter, 5);
                WariorRender.enabled = false;
                Warior.transform.position = OffScreen;
                Knight.transform.position = CharacterPosition;
                KnightRender.enabled = true;
                characterInt++;
                break;
            case 6:
                PlayerPrefs.SetInt(selectedCharacter, 6);
                KnightRender.enabled = false;
                Knight.transform.position = OffScreen;
                LBandit.transform.position = CharacterPosition;
                LBanditRender.enabled = true;
                characterInt++;
                ResetInt();
                break;*/
            default:
                ResetInt();
                break;
        }


    }
    
    public void Previous()
    {
        switch (characterInt)
        {
            case 1:
                PlayerPrefs.SetInt(selectedCharacter, 6);
                LBanditRender.enabled = false;
                LBandit.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                Warior.transform.position = newpos;
                WariorRender.enabled = true;
                characterInt--;
                ResetInt();
                break;
            case 2:
                PlayerPrefs.SetInt(selectedCharacter, 5);
                WariorRender.enabled = false;
                Warior.transform.position = OffScreen;
                LBandit.transform.position = CharacterPosition;
                LBanditRender.enabled = true;
                characterInt--;
                break;
            /*case 3:
                PlayerPrefs.SetInt(selectedCharacter, 4);
                SteamManRender.enabled = false;
                SteamMan.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                GraveRobber.transform.position = newpos;
                GraveRobberRender.enabled = true;
                characterInt--;
                break;
            case 4:
                PlayerPrefs.SetInt(selectedCharacter, 3);
                WoodcutterRender.enabled = false;
                WoodCutter.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                SteamMan.transform.position = newpos;
                SteamManRender.enabled = true;
                characterInt--;
                break;
            case 5:
                PlayerPrefs.SetInt(selectedCharacter, 2);
                WariorRender.enabled = false;
                Warior.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                WoodCutter.transform.position = newpos;
                WoodcutterRender.enabled = true;
                characterInt--;
                break;
            case 6:
                PlayerPrefs.SetInt(selectedCharacter, 1);
                KnightRender.enabled = false;
                Knight.transform.position = OffScreen;
                newpos = new Vector3(CharacterPosition.x, CharacterPosition.y + 1, CharacterPosition.z);
                Warior.transform.position = newpos;
                WariorRender.enabled = true;
                characterInt--;
                break;*/
            default:
                ResetInt();
                break;
        }
    }

    private void ResetInt()
    {
        if (characterInt >= 2)
        {
            characterInt = 1;
        }
        else
        {
            characterInt = 2;
        }
    }
    public void ChengScene()
    {
        PlayerPrefs.SetInt("ChooseChar", characterInt);
        PlayerPrefs.SetInt("WhatStage",3);
        SceneManager.LoadScene(3);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainchar : MonoBehaviour
{
    public Sprite WoodCutter,GraveRobber,SteamMan,Warior;
    private SpriteRenderer mySprite;
    private readonly string selectedCharacter ="SelectedCharacter";
 void Awake ()
 {
    mySprite = this.GetComponent<SpriteRenderer>();
 }
 void Start()
 {
    int getCharacter;

    getCharacter = PlayerPrefs.GetInt(selectedCharacter);

 switch(getCharacter)
 {

     case 1:
     mySprite.sprite = WoodCutter;
     break;

     case 2:
     mySprite.sprite = GraveRobber;
     break;

     case 3:
     mySprite.sprite = SteamMan;
     break;

     case 4:
     mySprite.sprite = Warior;
     break;

     default:
     mySprite.sprite = Warior;
     break;
     }
  }
}



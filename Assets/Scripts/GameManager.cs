using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameMode mode;

    void Awake(){
        Instance = this;
    }
    public void ChangeMode(GameMode newMode){
        mode = newMode;
        switch(mode){
            case GameMode.LastManStanding:
              
                break;
        }
    }
}

public enum GameMode{
    LastManStanding,
}

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
    void Update(){
        
    }
    public void StartMode(){
        switch(mode){
            case GameMode.LastManStanding:
                foreach(GameObject player in PlayerManager.Instance.players){
                    StartCoroutine(StartStun(player.GetComponent<Playermove>()));
                    player.GetComponent<Playermove>().stunned = true;
                    player.GetComponent<GamePlayer>().livesLeft = 3;
                }
                break;
        }
    }

    public void ModeEffects(){
        switch(mode){
            case GameMode.LastManStanding:
                Debug.Log("Mode Effects");
                int stillIn = 0;
                GameObject playerStillIn = null;
                foreach(GameObject player in PlayerManager.Instance.players){
                    if(player.GetComponent<GamePlayer>().livesLeft > 0){
                        stillIn += 1;
                        playerStillIn = player;
                    }
                }
                if(stillIn == 1 && playerStillIn){
                    playerStillIn.GetComponent<GamePlayer>().numWins += 1;
                    StartCoroutine(EndOfMode());
                }
                break;
        }
    }

    private IEnumerator StartStun(Playermove playermove){
        playermove.howLongYouAreStunned = 0.5f;
        playermove.stunned = true;
        yield return new WaitForSeconds(0.6f);
        playermove.howLongYouAreStunned = playermove.staticStunTime;
    }

    private IEnumerator EndOfMode(){
        yield return new WaitForSeconds(2);
        StartCoroutine(PlayerManager.Instance.SceneSwap());
    }
}

public enum GameMode{
    LastManStanding,
    KingOfTheHill,
    HoleInTheWall
}

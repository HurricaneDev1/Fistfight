using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameMode mode;
    public bool endedGame;

    void Awake(){
        Instance = this;
        mode = GameMode.LastManStanding;
    }
    void Update(){
        
    }
    public void StartMode(){
        switch(mode){
            case GameMode.LastManStanding:
                foreach(GameObject player in PlayerManager.Instance.players){
                    StartCoroutine(StartStun(player.GetComponent<Playermove>()));
                    player.GetComponent<GamePlayer>().livesLeft = 3;
                }
                break;
            case GameMode.KingOfTheHill:
                foreach(GameObject player in PlayerManager.Instance.players){
                    StartCoroutine(StartStun(player.GetComponent<Playermove>()));
                    player.GetComponent<GamePlayer>().livesLeft = 10000;
                    player.GetComponent<GamePlayer>().kingTime = 6;
                }
                break;
        }
        endedGame = false;
    }

    public void ModeEffects(){
        switch(mode){
            case GameMode.LastManStanding:
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
            case GameMode.KingOfTheHill:
                foreach(GameObject player in PlayerManager.Instance.players){
                    if(player.GetComponent<GamePlayer>().kingTime <= 0 && !endedGame){
                        endedGame = true;
                        player.GetComponent<GamePlayer>().numWins += 1;
                        StartCoroutine(EndOfMode());
                        player.GetComponent<GamePlayer>().onHill = false;
                    }
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

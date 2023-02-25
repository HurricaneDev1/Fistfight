using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField]private Transform respawn;
    [SerializeField]private Transform deathSpot;
    [SerializeField]private float respawnTime;
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            StartCoroutine(Respawn(col.gameObject));
        }
    }

    //Kills and respawns a player who touched lava @ need to add limited respawns
    IEnumerator Respawn(GameObject col){
        if(col.GetComponent<Playermove>())col.GetComponent<Playermove>().deathParticle.Play();

        yield return new WaitForSeconds(0.01f);
        col.transform.position = deathSpot.position;

        yield return new WaitForSeconds(respawnTime);
        GamePlayer curPlayer = col.GetComponent<GamePlayer>();
        if(curPlayer){
            curPlayer.livesLeft -= 1;
            if(curPlayer.livesLeft > 0){
                col.transform.position = PlayerManager.Instance.map.respawnPoints[Random.Range(0,PlayerManager.Instance.map.respawnPoints.Count)].position;
            }
            GameManager.Instance.ModeEffects();
        }else{
            col.transform.position = PlayerManager.Instance.map.respawnPoints[Random.Range(0,PlayerManager.Instance.map.respawnPoints.Count)].position;
        }
    }
}

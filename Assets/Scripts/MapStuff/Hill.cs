using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hill : MonoBehaviour
{
    void Update(){
        GameManager.Instance.ModeEffects();
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.GetComponent<GamePlayer>()){
            col.GetComponent<GamePlayer>().onHill = true;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.GetComponent<GamePlayer>()){
            col.GetComponent<GamePlayer>().onHill = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Transform deathSpot;
    public int moveSpeed;

    void Start(){
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.AddForce(moveSpeed * Vector2.right);
        Destroy(gameObject, 15);
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            col.transform.position = deathSpot.position;
            col.GetComponent<GamePlayer>().livesLeft -= 1;
            GameManager.Instance.ModeEffects();
        }
    }
}

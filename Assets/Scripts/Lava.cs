using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField]private Transform respawn;
    [SerializeField]private Transform deathSpot;
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            StartCoroutine(Respawn(col.gameObject));
        }
    }

    IEnumerator Respawn(GameObject col){
        col.transform.position = deathSpot.position;
        yield return new WaitForSeconds(2);
        col.transform.position = respawn.position;
    }
}

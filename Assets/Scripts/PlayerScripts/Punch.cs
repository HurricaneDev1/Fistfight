using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField]private float punchForce;
    [SerializeField]private float punchRadius;
    [SerializeField]private float upForce;
    [SerializeField]private float overallPower;
    [SerializeField]private Transform punchPoint;
    [SerializeField]private float punchGravDelay;

    //Does a punch attack
    public void PunchAction(float punchMod){
        //Gets a list of everybody in a circles area
        Collider2D[] col = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius);

        //Goes through every guy in the list
        foreach(Collider2D guy in col){
            Playermove move = guy.gameObject.GetComponent<Playermove>();

            if(guy.GetComponent<MenuController>()){
                StartCoroutine(guy.GetComponent<MenuController>().OnPunch());
            }
            //Sees if the guy has a rigidbody and is not static
            if(guy.gameObject.GetComponent<Rigidbody2D>() && gameObject != guy.gameObject && guy.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static){
                guy.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

                //Stuns other players and plays punch particle
                if(move) {
                    move.stunned = true;
                    move.punchParticle.Play();
                }

                //Pushes guy
                guy.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Playermove>().facingRight ? punchForce : -punchForce , upForce) * overallPower * punchMod);
            }
        }
    }

    //Turns off gravity of hit players for a short time
    private IEnumerator Grav(Playermove move){
        move.fallMultiplier = 0;
        yield return new WaitForSeconds(punchGravDelay);
        move.fallMultiplier = move.staticFallMultiplier;
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}

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

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PunchAction(float punchMod){
        Debug.Log("Punch");
        Collider2D[] col = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius);
        foreach(Collider2D guy in col){
            Playermove move = guy.gameObject.GetComponent<Playermove>();
            Debug.Log(guy.name);
            if(guy.gameObject.GetComponent<Rigidbody2D>() && gameObject != guy.gameObject && guy.gameObject.tag != "Ground" && guy.gameObject.tag != "Wall"){
                guy.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                if(move) {
                    move.stunned = true;
                    move.punchParticle.Play();
                }
                guy.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Playermove>().facingRight ? punchForce : -punchForce , upForce) * overallPower * punchMod);
            }
        }
    }

    private IEnumerator Grav(Playermove move){
        move.fallMultiplier = 0;
        yield return new WaitForSeconds(punchGravDelay);
        move.fallMultiplier = move.staticFallMultiplier;
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}

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

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J)){
            PunchAction();
        }
    }

    public void PunchAction(){
        Debug.Log("Punch");
        Collider2D[] col = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius);
        foreach(Collider2D guy in col){
            Debug.Log(guy.name);
            if(guy.gameObject.GetComponent<Rigidbody2D>() && guy.name != this.name && guy.gameObject.tag != "Ground" && guy.gameObject.tag != "Wall"){
                guy.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                guy.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Playermove>().facingRight ? punchForce : -punchForce , upForce) * overallPower);
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}

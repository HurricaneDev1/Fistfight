using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayer : MonoBehaviour
{
    public int livesLeft;
    public float kingTime;
    public bool onHill;
    public int numWins;
    public TextMeshProUGUI gameText;

    void Update(){
        if(onHill){
            kingTime -= Time.deltaTime;
        }
    }
}

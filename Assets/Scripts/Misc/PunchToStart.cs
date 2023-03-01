using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchToStart : MonoBehaviour
{
    public void OnPunch(){
        PlayerManager.Instance.StartSceneSwap();
        Debug.Log("Did it");
    }
}

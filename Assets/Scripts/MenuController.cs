using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI gameModeText;
    [SerializeField]private List<string> names = new List<string>(){
        ""
    };

    void Start(){
        OnPunch();
    }

    private void OnPunch(){
        GameManager.Instance.ChangeMode(GameMode.LastManStanding);
    }
}

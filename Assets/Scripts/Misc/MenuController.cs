using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI gameModeText;
    [SerializeField]private ParticleSystem onHit;
    private bool changing;
    private List<string> names = new List<string>(){
        "Last Man Standing",
        "King of the hill",
        "Hole in the wall"
    };
    [SerializeField]private int currentNameIndex;
    void Start(){
        currentNameIndex = (int)GameManager.Instance.mode;
        gameModeText.text = names[currentNameIndex];
    }
    public IEnumerator OnPunch(){
        if(changing == false){
            changing = true;
            currentNameIndex = (currentNameIndex + 1) % names.Count;
            // onHit.Play();
            GameManager.Instance.mode = (GameMode)currentNameIndex;
            gameModeText.text = names[currentNameIndex];
            yield return new WaitForSeconds(0.2f);
            changing = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public int timeBeforeSwap;
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(WaitTillTime());
    }

    private IEnumerator WaitTillTime(){
        yield return new WaitForSeconds(timeBeforeSwap);
        PlayerManager.Instance.StartSceneSwap(null);
    }
}

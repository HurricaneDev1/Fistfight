using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public List<GameObject> players = new List<GameObject>();
    public List<Transform> usedSpawnPoints = new List<Transform>();
    public Map map;

    void Awake(){
        Instance = this;
    }

    void Start(){
        map = FindObjectOfType<Map>();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Y)){
            StartCoroutine(SceneSwap());
        }

        if(map == null){
            map = FindObjectOfType<Map>();
        }
    }

    //Adds a player to the list of them and spawns it in
    public void AddPlayer(GameObject player){
        players.Add(player);
        Spawn(player);
    }
    //Spawns players in spawn points @ should make random
    public void Spawn(GameObject player){
        foreach(Transform spawn in map.spawnPoints){
            if(!usedSpawnPoints.Contains(spawn)){
                player.transform.position = spawn.position;
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                usedSpawnPoints.Add(spawn);
                break;
            }
        }
    }

    public void StartSceneSwap(){
        StartCoroutine(SceneSwap());
    }
    //Has some logic for when the scene swaps over. Like spawning players and resetting values
    public IEnumerator SceneSwap(){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }else if(SceneManager.GetActiveScene().name == "WaitRoom"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + ((int)GameManager.Instance.mode + 1));
        }else{
            SceneManager.LoadScene("WaitRoom");
        }
        yield return new WaitForSeconds(0.00001f);
        usedSpawnPoints = new List<Transform>();
        map = FindObjectOfType<Map>();
        foreach(GameObject guy in players){
            Spawn(guy);
        }
        GameManager.Instance.StartMode();
    }
}

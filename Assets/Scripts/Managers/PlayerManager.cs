using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public List<GameObject> players = new List<GameObject>();
    public Map map;

    void Awake(){
        Instance = this;
    }

    void Start(){
        map = FindObjectOfType<Map>();
    }

    void Update(){
        // if(Input.GetKeyDown(KeyCode.Y)){
        //     StartCoroutine(SceneSwap());
        // }

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
        Transform newSpawn = map.spawnPoints[Random.Range(0, map.spawnPoints.Count)];
        if(newSpawn != null){
            player.transform.position = newSpawn.position;
            map.spawnPoints.Remove(newSpawn);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
        // foreach(Transform spawn in map.spawnPoints){
            //     if(!usedSpawnPoints.Contains(spawn)){
            //         player.transform.position = spawn.position;
            //         player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            //         usedSpawnPoints.Add(spawn);
            //         usedSpawn = spawn;
            //         break;
            //     }
            //     Debug.Log("Spawning");
            // }
    }

    public void StartSceneSwap(GameObject priorityPlayer){
        StartCoroutine(SceneSwap(priorityPlayer));
    }
    //Has some logic for when the scene swaps over. Like spawning players and resetting values
    public IEnumerator SceneSwap(GameObject priorityPlayer){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }else if(SceneManager.GetActiveScene().name == "WaitRoom"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + ((int)GameManager.Instance.mode + 1));
        }else if(SceneManager.GetActiveScene().name != "WinScreen"){
            SceneManager.LoadScene("WinScreen");
        }else{
            SceneManager.LoadScene("WaitRoom");
        }
        yield return new WaitForSeconds(0.00001f);
        map = FindObjectOfType<Map>();
        if(priorityPlayer){
            priorityPlayer.transform.position = map.spawnPoints[0].position;
            map.spawnPoints.Remove(map.spawnPoints[0]);
            priorityPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
        foreach(GameObject guy in players){
            if(guy != priorityPlayer){
                Spawn(guy);   
            }
        }
        GameManager.Instance.StartMode();
    }
}

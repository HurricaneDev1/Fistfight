using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField]private GameObject projectile;
    [SerializeField]private int numWall;
    [SerializeField]private List<GameObject> bullets = new List<GameObject>();
    [SerializeField]private Transform deathSpot;
    private float projectileTimer = 2;
    private int numTimesGone;
    // Update is called once per frame
    void Start()
    {
        
    }

    void Update(){
        if(projectileTimer <= 0){
            SummonBullets();
            projectileTimer = 8 - (numTimesGone * 0.5f);
            if(projectileTimer < 0.3)projectileTimer = 0.3f;
            numTimesGone ++;
        }else{
            projectileTimer -= Time.deltaTime;
        }
    }

    void SummonBullets(){
        bullets.Clear();
        for(int i = 0; i < numWall; i++){
            bullets.Add(Instantiate(projectile, new Vector2(transform.position.x,transform.position.y - i), Quaternion.identity));
        }

        foreach(GameObject bullet in bullets){
            bullet.GetComponent<MoveForward>().deathSpot = deathSpot;
        }
        int newRand = Random.Range(1, bullets.Count - 1);
        Destroy(bullets[newRand], 0.1f);
        Destroy(bullets[newRand + 1], 0.1f);
        Destroy(bullets[newRand - 1], 0.1f);
    }
}

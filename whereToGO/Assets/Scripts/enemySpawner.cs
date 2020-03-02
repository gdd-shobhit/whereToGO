using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform placeToSpawnFrom;
    public Transform player;
    public bool spawner=false;
    public float timer;
    // Update is called once per frame
    private void Start()
    {
        timer = 0;
        enemyPrefab.GetComponent<HomeObjects>().targetTransform = player;
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(Time.timeSinceLevelLoad);
        if(timer>5)
        {
            spawner = true;
            timer = 0;
        }
        if (spawner == true)
        {
            Instantiate(enemyPrefab, placeToSpawnFrom.position, Quaternion.Inverse(Quaternion.identity));
            spawner = false;
           
        }
        
    }
}

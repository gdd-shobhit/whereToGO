using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform placeToSpawnFrom;
    public Transform player;
    public bool spawner=false;
    // Update is called once per frame
    private void Start()
    {
        enemyPrefab.GetComponent<HomeObjects>().targetTransform = player;
    }

    void Update()
    {
        //Debug.Log(Time.timeSinceLevelLoad);
        if((int)Time.timeSinceLevelLoad % 5 == 0)
        {
            spawner = true;
        }
        if ((int)Time.timeSinceLevelLoad % 4 == 0 && spawner == true)
        {
            Instantiate(enemyPrefab, placeToSpawnFrom.position, Quaternion.Inverse(Quaternion.identity));
            spawner = false;
            //enemyPrefab.gameObject.GetComponent("Home Objects").GetComponent
        

        }

        /*
         * 
         * 
         * 
         * */
        
    }
}

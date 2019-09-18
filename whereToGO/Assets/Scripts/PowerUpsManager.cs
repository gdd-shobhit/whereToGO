using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public GameObject firePower;
    public GameObject frostyPower;

    // Start is called before the first frame update
    void Start()
    {
        firePower.SetActive(true);
        frostyPower.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Time.timeSinceLevelLoad % 5 == 0 && firePower.activeSelf == false)
        //{
        //    firePower.SetActive(true);
        //}

        //if (Time.timeSinceLevelLoad % 5 == 0 && frostyPower.activeSelf == false)
        //{
        //    frostyPower.SetActive(true);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firePower.activeSelf)
        {
            Debug.Log("activeSelf working");
            PickUp(firePower);
        }

        if (frostyPower.activeSelf)
        {
            PickUp(frostyPower);
        }
    }

    void PickUp(GameObject power)
    {
        power.SetActive(false);

    }
}

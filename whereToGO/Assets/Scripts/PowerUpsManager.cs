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

        StartCoroutine(RespawningPowerInSeconds(power, 5));

    }

    IEnumerator RespawningPowerInSeconds(GameObject power,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        power.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator PickUpRoutine()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(true);
        StopCoroutine(PickUpRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(PickUpRoutine());
        gameObject.SetActive(false);
    }
}

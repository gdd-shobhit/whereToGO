using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ParticleSystem effect;
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


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in there");
        if (other.tag == "alive")
        {
            Instantiate(effect, transform.position, transform.rotation);
        }
    }
}

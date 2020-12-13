using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ParticleSystem effect;

    IEnumerator PickupRoutine(float time)
    {
        ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        ps.Stop();
        collider.enabled = false;
        yield return new WaitForSeconds(time);
        ps.Play();
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "alive")
        {
            StartCoroutine(PickupRoutine(2));
            Instantiate(effect, gameObject.transform.position, Quaternion.identity);
            if(gameObject.tag == "frost")
            {
                collision.GetComponent<playerMovement>().frostStance = true;
                return;
            }
            collision.GetComponent<playerMovement>().fireStance = true;
        }
        
    }
}

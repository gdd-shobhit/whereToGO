using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawRotator : MonoBehaviour
{
    public GameObject player;
    public BoxCollider2D boxCollider;
    private float timeMultiplier = 1.0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.Rotate(Vector3.forward, -10f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {                
           collision.gameObject.tag = "dead";       
    }
}

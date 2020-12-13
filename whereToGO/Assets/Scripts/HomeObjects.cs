using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomeObjects : MonoBehaviour
{
    
    public Transform targetTransform;

    private Rigidbody2D rb;

    public ParticleSystem ps;
    public float speed = 5f;
    public bool isRight = true;
    public float timer = 0;
    private float timeMultiplier = 1.0f;
    public float rotateSpeed =200f;
    // Start is called before the first frame update
    
    void Start()
    {
        //targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (targetTransform.gameObject.GetComponent<playerMovement>().frostStance)
        {
            timeMultiplier *= 0.2f;
        }
        timer += Time.deltaTime;
        DeathMangager();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetTransform != null)
        {

        Vector2 direction = (Vector2)targetTransform.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed * timeMultiplier;

        }
        if (timer>10)
        {
            gameObject.tag = "dead";
            timer = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "alive")
        {
            if (!collision.gameObject.transform.GetChild(2).gameObject.activeSelf)
                collision.gameObject.tag = "dead";
            gameObject.tag = "dead";
        }
    }
    void DeathMangager()
    {
        if (gameObject.tag == "dead")
        {
            //Vector3 bloodVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}

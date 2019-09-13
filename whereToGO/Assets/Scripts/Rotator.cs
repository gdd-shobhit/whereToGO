using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject player;
    public float angle;
    public Vector2 forcePostion;
    public Vector2 force;
    public Transform checkGround;
    public bool playerIsGrounded=true;
    public LayerMask whatIsThis;
    void FixedUpdate()
    {
        //paddle.AddTorque(100f,ForceMode2D.Force);

        if( player.GetComponent<Rigidbody2D>().transform.position.x > 0 && playerIsGrounded==true)
        {
            gameObject.transform.Rotate(Vector3.forward, -10f * Time.deltaTime);
        }
        else if(player.GetComponent<Rigidbody2D>().transform.position.x < 0 && playerIsGrounded == true)
        {
            gameObject.transform.Rotate(Vector3.forward, 10f * Time.deltaTime);
        }
    }
    private void Update()
    {
        playerIsGrounded = Physics2D.OverlapCircle(checkGround.position, 0.2f, whatIsThis);

    }

}

using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed=2f;
    private int extraJumps=1;
    public int extraJumpValue;
    private float movement=0f;
    public bool facingRight = true;
    public bool isGrounded;
    public bool isWalled;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public Transform feetPos;
    public float feetRadius;
    public float jumpForce=12f;
    public Vector2 vel;
    private Vector2 clampedVel;
    public ParticleSystem skull;
    public ParticleSystem deathBlood;
    private bool fireStance;
    //public int localExtraJumps;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpValue;
        fireStance = false;
    }

    void FixedUpdate()
    {
        movement = Input.GetAxis("Horizontal");
        vel = rb.velocity;
        //clampedVel = Vector2.ClampMagnitude(rb.velocity, -0.2f);
        //rb.velocity = clampedVel;
        rb.AddForce(new Vector2(movement * speed*25, rb.velocity.y), ForceMode2D.Force);

        // checking for fire stance
        if (Input.GetKeyDown(KeyCode.F))
        {
            FireStance();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        //rb.transform.position = rb.transform.position+new Vector3(movement * speed * Time.deltaTime, rb.velocity.y, 0);
        DeathMangager();
        
        if (movement>0 && facingRight == false)
        {           
            Flip();
        }
        else if(movement<0 && facingRight == true)
        {          
            Flip();
        }
        if (gameObject.transform.position.y < -15f)
        {
            gameObject.transform.position = new Vector3(0, 8, 1);
        }
    }
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, feetRadius, whatIsGround);
        isWalled= Physics2D.OverlapCircle(feetPos.position, feetRadius, whatIsWall);
        if (isGrounded == true || isWalled==true )
        {
            extraJumps = extraJumpValue;
            
        }        
        if(Input.GetKeyDown(KeyCode.W) && extraJumps > 0)
        {
            //rb.velocity = transform.up * jumpForce * Time.deltaTime;
            rb.AddForce(new Vector2(0, jumpForce*100f), ForceMode2D.Force);
            extraJumps--;
        }
        else if(Input.GetKeyDown(KeyCode.W) && extraJumps == 0 && (isGrounded == true || isWalled==true))
        {
            rb.AddForce(new Vector2(0, jumpForce * 100f), ForceMode2D.Force);
            //skull.Play();
        }

    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        //Instantiate(skull);
        //skull.Play() ;
    }
    void DeathMangager()
    {
        if (gameObject.tag == "dead")
        {
            Vector3 bloodVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
            Instantiate(deathBlood, bloodVector, Quaternion.identity);
            Destroy(gameObject);            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "deathObjectf")
        //{
        //    collision.gameObject.tag = "dead";
        //}      
    }

    void FireStance()
    {
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }
}

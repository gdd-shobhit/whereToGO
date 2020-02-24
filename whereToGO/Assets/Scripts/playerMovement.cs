using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public GameObject grapplePrefab;
    private GameObject grapple;
    private float GRAPPLE_DISTANCE = 4.0f;
    private Vector3 grappleDestination;

    public Rigidbody2D rb;
    public float speed=2f;
    private int jumps=2;
    public int extraJumpValue;
    private float movement=0f;
    private bool isGrappling = false;
    private bool grappleHit = false;
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
        grapple = Instantiate(grapplePrefab, transform.position, Quaternion.identity);
        grapple.SetActive(false);
        jumps = extraJumpValue;
        fireStance = false;
    }

    void FixedUpdate()
    {
        if (!isGrappling)
        {
            InitializeGrapple();
            Movement();
        }
        else
        {
            ShootGrapple();
        }
        FireStance();
        //DeathMangager();

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void Movement()
    {
        movement = Input.GetAxis("Horizontal");
        vel = rb.velocity;
        rb.AddForce(new Vector2(movement * speed * 25, rb.velocity.y), ForceMode2D.Force);

        isWalled = Physics2D.OverlapCircle(feetPos.position, feetRadius, whatIsWall);
        RaycastHit2D hit = Physics2D.Raycast(feetPos.position, Vector2.down);
        if (hit)
            if (hit.rigidbody.gameObject.tag != "alive") isGrounded = hit.distance < 0.01f;

        if (isGrounded || isWalled) jumps = extraJumpValue;
        Debug.Log(hit.distance);
        if (Input.GetKeyDown(KeyCode.W) && jumps > 0)
        {
            Debug.Log(jumps);
            rb.AddForce(new Vector2(0, jumpForce * 100f), ForceMode2D.Force);
            jumps--;
        }

        if (movement > 0 && facingRight == false) Flip();
        else if (movement < 0 && facingRight == true) Flip();
    }

    void DeathMangager()
    {
        if (gameObject.tag == "dead")
        {
            Vector3 bloodVector = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
            Instantiate(deathBlood, bloodVector, Quaternion.identity);
            Destroy(gameObject);
        }

        if (gameObject.transform.position.y < -15f)
        {
            gameObject.transform.position = new Vector3(0, 4, 1);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
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
        // checking for fire stance
        if (Input.GetKeyDown(KeyCode.F)) 
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        if (Input.GetKeyUp(KeyCode.F))
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }

    //Initialize the grapple
    void InitializeGrapple()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
            grappleDestination = mousePos - new Vector3(transform.position.x, transform.position.y, -1);
            grappleDestination.Normalize();
            grappleDestination = (grappleDestination * GRAPPLE_DISTANCE) + transform.position;
            grapple.transform.position = transform.position;
            grapple.SetActive(true);
            isGrappling = true;
            rb.gravityScale = 0;
            rb.velocity *= 0.1f;
        }
    }

    //Called while grappling
    void ShootGrapple()
    {
        if (!grappleHit)
        {
            grapple.transform.position = Vector3.Lerp(grapple.transform.position, grappleDestination, 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(grapple.transform.position, grappleDestination, 0.05f);
            if (hit)
                if (hit.rigidbody.gameObject.tag != "alive")
                    grappleHit = true;
            if (Vector2.Distance(grappleDestination, grapple.transform.position) < 0.2f)
                EndGrapple();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, grappleDestination, 0.1f);
            if (Vector2.Distance(grapple.transform.position, transform.position) < 0.2f)
                EndGrapple();
        }
    }

    private void EndGrapple()
    {
        isGrappling = false;
        grappleHit = false;
        grapple.SetActive(false);
        rb.gravityScale = 1;
    }
}

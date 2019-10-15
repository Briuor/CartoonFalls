using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool isJumping = false;
    private bool isGrounded;
    public float jumpForce = 10f;
    [SerializeField]
    private float moveInput;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask ground;

    private int extraJumps;
    public int extraJumpsValue;
    public float vel = 0;

    public bool player2 = false;

    //Animator
    public Animator anim;
    public Transform jumpDust;

    // Damage

    public bool isPunching;
    public bool damaged;
    public float weakness = 0;
    public bool knockingBack = false;
    bool normalizingKnockback = false;
    public bool isDead;
    public Transform punchPlotter;


    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        isGrounded = false;
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Movement()
    {
        string xAxis = player2 ? "Horizontal_2" : "Horizontal_1";
        string yAxis = player2 ? "Vertical_2"   : "Vertical_1";
        
        KeyCode up    = player2 ? KeyCode.I : KeyCode.W;
        KeyCode down  = player2 ? KeyCode.K : KeyCode.S;
        KeyCode left  = player2 ? KeyCode.J : KeyCode.A;
        KeyCode right = player2 ? KeyCode.L : KeyCode.D;

        if(!knockingBack){
            moveInput = Input.GetAxis(xAxis) * speed;

            if (Input.GetKeyDown(up))
            {
                if(extraJumps > 0){
                    rb.velocity = Vector2.up * jumpForce;
                    extraJumps--;
                    if(anim.GetBool("isJumping") || anim.GetBool("isFalling")){
                        Instantiate(jumpDust, new Vector3(transform.position.x, (transform.position.y), 0), Quaternion.identity);
                    }
                }
            }else if(Input.GetKeyDown(up) && extraJumps == 0 && isGrounded == true)
            {
                rb.velocity = Vector2.up * jumpForce;
            }
        }

    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
        
        Movement();
        Punch();
        AnimationController();
        if(!normalizingKnockback)
        {
            StartCoroutine(NormalizeKnockback());
        }
        // if(isGrounded == true) anim.SetBool("isJumping", false);

        rb.velocity = new Vector2(moveInput, rb.velocity.y);
        vel = rb.velocity.y;

    }

    private void AnimationController()
    {
        KeyCode left  = player2 ? KeyCode.J : KeyCode.A;
        KeyCode right = player2 ? KeyCode.L : KeyCode.D;

        if(moveInput < 0 && transform.rotation.y != 180) 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }else if(moveInput > 0 ){
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(left) || Input.GetKey(right))
        {
            anim.SetBool("isMoving", true);
        }else{
            anim.SetBool("isMoving", false);
        }
        
        if(vel > 0)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
        }else 
        if(vel < 0){
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }else{
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }
        if(isDead)
        {
            isDead = !isDead;
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void Punch()
    {
        KeyCode punch = player2 ? KeyCode.Comma : KeyCode.X;

                
        isPunching = !isPunching ? Input.GetKeyDown(punch) : isPunching;
        punchPlotter.gameObject.SetActive(isPunching);

        if(isPunching) StartCoroutine(ReleasePunch());
    }

    void Damage(Collider2D other)
    {
        int side = transform.rotation.y == 180 ? -1 : 1;
        other.gameObject.GetComponent<PlayerController>().ReceiveDamage(gameObject);
    }

    public void ReceiveDamage(GameObject other)
    {
        float direction = other.transform.rotation.y == -1 ? -1 : 1;

        Debug.Log(other.gameObject.transform.rotation);

        weakness += 0.1F;
        float multiplier = (0.5F + weakness) * jumpForce;

        // Debug.Log("I ("+gameObject.name+") Received Damage!");

        rb.AddForce(new Vector2(1,4) * multiplier);
        StartCoroutine(Knockback(direction));
        if(isGrounded && !knockingBack){
            StopCoroutine(Knockback(0));
            moveInput = 0;
            knockingBack = false;
        }
    }

    IEnumerator Knockback(float direction)
    {
       
        knockingBack = true;
        moveInput = direction * speed * (1 + weakness);

        yield return new WaitForSeconds(0.2F + weakness);

        moveInput = 0;
        knockingBack = false;
    }

    IEnumerator ReleasePunch()
    {
        yield return new WaitForSeconds(0.25F);
        isPunching = false;
    }
        private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.name == "SafeArea"){
            isDead = true;
            Debug.Log(other.gameObject.name + gameObject.name);
        }

    }



    IEnumerator NormalizeKnockback()
    {
        normalizingKnockback = true;
        while(weakness > 0){
            yield return new WaitForSeconds(1);
            weakness -= 0.03F;
        }
        weakness = 0;
        normalizingKnockback = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.tag == "Player" && !GameObject.ReferenceEquals(other.gameObject, gameObject)){
            Damage(other);
        }
    }

}

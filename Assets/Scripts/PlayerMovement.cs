using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public bool isJumping = false;
    private bool isGrounded;
    public float jumpForce = 10f;
    private float moveInput;
    private Collider2D playerCollider;
    private Rigidbody2D rb;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask ground;

    private int extraJumps;
    public int extraJumpsValue;
    public float vel = 0;


    //Animator
    public Animator anim;
    public Transform jumpDust;

    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        isGrounded = false;
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
        moveInput = Input.GetAxis("Horizontal") * speed;

        if(moveInput < 0 && transform.rotation.y != 180) 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }else if(moveInput > 0 ){
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMoving", true);
        }else{
            anim.SetBool("isMoving", false);
        }
        
        if(vel > 0 && vel < 11)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
        }else if(vel < 0 && vel > -11){
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
        }else{
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if(extraJumps > 0){
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
                if(anim.GetBool("isJumping") || anim.GetBool("isFalling")){
                    Instantiate(jumpDust, new Vector3(transform.position.x, (transform.position.y), 0), Quaternion.identity);
                }
            }
        }else if(Input.GetKeyDown(KeyCode.W) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
        

        // if(isGrounded == true) anim.SetBool("isJumping", false);

        rb.velocity = new Vector2(moveInput, rb.velocity.y);
        vel = rb.velocity.y;

    }



    // Update is called once per frame
    void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }
    }

}

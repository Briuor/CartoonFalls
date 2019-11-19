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
    private int moveDirection = 1;
    private Collider2D playerCollider;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask ground;
    [SerializeField]
    private int extraJumps;
    public int extraJumpsValue;
    [SerializeField]
    private float jumpDelay = 0;
    public float vel = 0;
    private int numberOfFalls = 0;


    //Controller
    KeyCode up;
    KeyCode down;
    KeyCode left;
    KeyCode right;
    KeyCode punch;
    KeyCode defense;

    public bool player2 = false;

    //Animator
    public Animator anim;
    public Transform jumpDust;

    //Sounds
    public AudioClip[] punchClips;
    public AudioClip[] jumpClips;
    public AudioClip[] dodgeClips;
    public AudioClip doubleJumpClip;
    
    // Damage
    public bool isPunching;
    private bool superPunch = false;
    public bool damaged;
    public float weakness = 0;
    public bool knockingBack = false;
    bool normalizingKnockback = false;
    public bool isDead;
    public Transform punchPlotter;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        extraJumps = extraJumpsValue;
        isGrounded = false;
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        up    = player2 ? KeyCode.I : KeyCode.W;
        down  = player2 ? KeyCode.K : KeyCode.S;
        left  = player2 ? KeyCode.J : KeyCode.A;
        right = player2 ? KeyCode.L : KeyCode.D;
        punch = player2 ? KeyCode.M : KeyCode.F;

    }

    private void Movement()
    {
        string xAxis = player2 ? "Horizontal_2" : "Horizontal_1";
        string yAxis = player2 ? "Vertical_2"   : "Vertical_1";
        
        

        if(!knockingBack){
            moveInput = Input.GetAxis(xAxis) * speed * moveDirection;

            if (Input.GetKeyDown(up))
            {

                if(extraJumps > 0 && (Time.time - jumpDelay) > 0.1f){
                    jumpDelay = Time.time;
                    rb.velocity = Vector2.up * jumpForce;
                    if(!isGrounded){
                        extraJumps--;
                    }
                    if(anim.GetBool("isJumping") || anim.GetBool("isFalling")){
                        PlaySound("doublejump");
                        Instantiate(jumpDust, new Vector3(transform.position.x, (transform.position.y), 0), Quaternion.identity);
                    }else{
                        PlaySound("jump");
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
        if(isGrounded == true) anim.SetBool("isJumping", false);

        rb.velocity = new Vector2(moveInput, rb.velocity.y);
        vel = rb.velocity.y;

    }

    private void AnimationController()
    {

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
        
        if(vel > 0 && !isGrounded)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
        }else 
        if(vel < 0 && !isGrounded){
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
        isJumping = !isGrounded;
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
        if(isDead)
        {
            isDead = !isDead;
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void PlaySound(string action)
    {
        AudioClip selectedClip = null;
        switch(action)
        {
            case "punch":
                selectedClip = punchClips[Random.Range(0, punchClips.Length-1)];
            break;  
             
            case "jump":
                selectedClip = jumpClips[Random.Range(0, jumpClips.Length-1)];
            break;

            case "doublejump":
                selectedClip = doubleJumpClip;
            break;

            case "dodge":
                selectedClip = dodgeClips[Random.Range(0, dodgeClips.Length-1)];                
            break;
        }
            audioSource.PlayOneShot(selectedClip);
    }

    private void Punch()
    {
        if(!isPunching && Input.GetKeyDown(punch))
        {
            isPunching = true;
            PlaySound("punch");
        }

        punchPlotter.gameObject.SetActive(isPunching);

        if(isPunching) StartCoroutine(ReleasePunch());
    }

    void Damage(Collider2D other)
    {
        int side = transform.rotation.y == 180 ? -1 : 1;
        other.gameObject.GetComponent<PlayerController>().ReceiveDamage(gameObject, superPunch);
    }

    public void ReceiveDamage(GameObject other, bool isSuperPunch)
    {
        float direction = other.transform.rotation.y == -1 ? -1 : 1;

        Debug.Log(other.gameObject.transform.rotation);

        float multiplier = 0f;

        if(isSuperPunch){
            weakness += 0.1F; 
            multiplier = (0.5F + weakness) * jumpForce * 2;
        } else{
            weakness += 0.1F;     
            multiplier = (0.5F + weakness) * jumpForce;
        }

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
            numberOfFalls += 1;
            Debug.Log(gameObject.name + "has fall");
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
        
        if(other.CompareTag("Player") && !GameObject.ReferenceEquals(other.gameObject, gameObject)){
            Damage(other);
        }
    }

    // Player powerUp functions
    public void PowerUpOn(PowerUpType powerUpType, float duration)
    {
        // Apply powerUp effect
        switch(powerUpType)
        {
            case PowerUpType.TRIPLE_JUMP:
                Debug.Log("TripleJump condition!");
                StartCoroutine(this.TripleJumpPowerUp(duration));
                break;
            
            case PowerUpType.SUPER_PUNCH:
                Debug.Log("SuperPunch condition!");
                StartCoroutine(this.SuperPunchPowerUp(duration));
                break;

            case PowerUpType.ULTRA_RIGID:
                Debug.Log("UltraRigid condition!");
                StartCoroutine(this.UltraRigidPowerUp(duration));
                break;

            case PowerUpType.ULTRA_SLOW:
                Debug.Log("UltraSlow condition!");
                StartCoroutine(this.UltraSlowPowerUp(duration));
                break;

            case PowerUpType.ULTRA_SPEED:
                Debug.Log("UltraSpeed condition!");
                StartCoroutine(this.UltraSpeedPowerUp(duration));
                break;

            case PowerUpType.FEATHER_WEIGHT:
                Debug.Log("FeatherWeight condition!");
                StartCoroutine(this.FeatherWeightPowerUp(duration));
                break;

            case PowerUpType.CONFUSED:
                Debug.Log("Confused condition!");
                StartCoroutine(this.ConfusedPowerUp(duration));    
                break;
            case PowerUpType.BAD_LUCKY:
                Debug.Log("The floor is Lava condition!");
                StartCoroutine(this.BadLuckyPowerUp(duration));
                break;
        }
    }
    public IEnumerator TripleJumpPowerUp(float time)
    {
        this.extraJumpsValue = 2;
        yield return new WaitForSeconds(time);
        this.extraJumpsValue = 1;
    }

    public IEnumerator SuperPunchPowerUp(float time)
    {
        superPunch = true;
        yield return new WaitForSeconds(time);
        Debug.Log("Unbuff!");
        superPunch = false;
    }

    public IEnumerator UltraRigidPowerUp(float time)
    {
        this.transform.localScale *= 2;
        yield return new WaitForSeconds(time);
        Debug.Log("Unbuff!");
        this.transform.localScale /= 2;
    }

    public IEnumerator UltraSlowPowerUp(float time)
    {   
        speed /= 2;
        yield return new WaitForSeconds(time);
        Debug.Log("Unbuff!");
        speed *= 2;
    }

    public IEnumerator UltraSpeedPowerUp(float time)
    {
        float save = speed;
        speed *= 3;
        yield return new WaitForSeconds(time);
        Debug.Log("Unbuff!");
        speed = save;
    }

    public IEnumerator FeatherWeightPowerUp(float time)
    {
        float save = weakness;
        weakness = 1.5f;
        yield return new WaitForSeconds(time);
        Debug.Log("Unbuff!");
        weakness = save;
    }

    public IEnumerator ConfusedPowerUp(float time)
    {
        moveDirection = -1;
        yield return new WaitForSeconds(time);
        Debug.Log("Unbuff!");
        moveDirection = 1;
    }

    public IEnumerator BadLuckyPowerUp(float time)
    {

        yield return new WaitForSeconds(time);
        
        if(isGrounded)
            isDead = true;
    }
}

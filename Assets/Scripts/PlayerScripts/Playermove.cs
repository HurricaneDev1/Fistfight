using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    [Header("Horizontal Movement")]
        public float moveSpeed = 10;
        public Vector2 direction;
        public bool facingRight = true;

    [Header("Physics")]
        public float maxSpeed;
        public float linearDrag = 4f;
        public float gravity = 1;
        public float fallMultiplier = 5;
        public float staticFallMultiplier;
        public float maxVerticalSpeed;

    [Header("Jump Stuff")]
        public float jumpHeight;
        public float jumpDelay = 0.25f;
        private float jumpTimer;
        private float coyoteTimer;

    [Header("Dash")]

        public bool stunned = true;
        private bool stunRecovery = false;
        [SerializeField]private float headBopJumpAmount;
        [SerializeField]private float headBopSlamAmount;
        [SerializeField]private float headBopRadius;
        [SerializeField]private Transform footCenter;

        public float howLongYouAreStunned = 1f;
        public float staticStunTime = 0.2f;
        private int dashesLeft = 1;

        [SerializeField]private float dashSpeed;
        private bool dashing = false;

    [Header("Player Components")]
        private Rigidbody2D rb;
        private SpriteRenderer spri;
        public LayerMask groundLayer;
        public LayerMask playerLayer;
        public GameObject player;
        public PlayerInput playerInput;
        private InputAction playerObj;
        public ParticleSystem punchParticle;
        public ParticleSystem deathParticle;
        public ParticleSystem dash;

    [Header("Player Collision")]
        public bool onGround = false;
        public Vector3 colliderOffset;
        [SerializeField]private float distanceToGround;
    
    private void Awake(){
        playerInput = new PlayerInput();
    }

    void OnEnable(){
        playerInput.Enable();
    }

    void OnDisable(){
        playerInput.Disable();
    }
    
    void Start()
    {
        spri = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        staticFallMultiplier = fallMultiplier;
        PlayerManager.Instance.AddPlayer(gameObject);
    }

    void Update()
    {
        if (onGround) {
            dashesLeft = 1;
        }
        bool wasOnGround = onGround;
        //Casts 2 raycasts down to see if the player is on the ground
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, distanceToGround, groundLayer) || Physics2D.Raycast(transform.position + new Vector3(-colliderOffset.x,colliderOffset.y,0), Vector2.down, distanceToGround, groundLayer);

        //Starts a coyote time after you leave ground
        if(wasOnGround && !onGround && rb.velocity.y < 0){
            coyoteTimer = Time.time + 0.15f;
        }

        //Checks to see if their is a player below you; if their is, do a headbop
        Collider2D[] guy = Physics2D.OverlapCircleAll(footCenter.position, headBopRadius, playerLayer);
        foreach(Collider2D col in guy){
            if(col && col.gameObject != gameObject){
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * headBopJumpAmount, ForceMode2D.Impulse);
                col.GetComponent<Rigidbody2D>().velocity = new Vector2(col.GetComponent<Rigidbody2D>().velocity.x,0);
                col.GetComponent<Rigidbody2D>().AddForce(Vector2.down * headBopSlamAmount, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        //Moves character and checks to see if it should change the gravity and drag
        moveCharacter();
        modifyPhysics();

        if(stunned == true && stunRecovery == false) {
            StartCoroutine(stunnedTimer());
        }

        if(Mathf.Abs(rb.velocity.y) > maxVerticalSpeed){
            rb.velocity = new Vector2(rb.velocity.x , Mathf.Sign(rb.velocity.y) * maxVerticalSpeed);
        }

        //If the jumptimer is still active since you inputted and you still have your coyote time/on the ground you jump
        if(jumpTimer > Time.time && (onGround || coyoteTimer > Time.time)){
            Jump();
        }
    }
    //Moves the player based on input
    public void moveCharacter()
    {
        if(stunned == false){
            float horizontal = direction.x;
            //Adds a force the direction inputted to move the player
            rb.AddForce(Vector2.right * horizontal * moveSpeed);

            //Checks to see which the direction the player is moving and which direction they are facing than flips if their not the same
            if((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)){
                Flip();
            }
            if(Mathf.Abs(rb.velocity.x) > maxSpeed){
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }

    //Starts to see if you can jump
    public void StartJump(){
        //if(jumpTimer < Time.time){
            jumpTimer = Time.time + jumpDelay;
        //}
    }
    //Jump action
    void Jump()
    {
        //Makes the character jump
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

        //Resets coyote and jump timer to prevent additional jumps
        jumpTimer = 0;
        coyoteTimer = 0;

    }

    //Flips the player
    void Flip(){
        //Changes the facing right variable to its opposite than flips the player
        facingRight = !facingRight;
        player.transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    //Messes with the players drag and stuff for better movement
    void modifyPhysics(){
        bool changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if(onGround){
            //when you turn around or stop inputting your drag will be increased
            if(Mathf.Abs(direction.x) < 0.4f || changingDirection){
                rb.drag = linearDrag;
            }
            else{
                rb.drag = 0;
            }
            rb.gravityScale = 0;
        }
        else{
            //Applies gravity and drag for jump, which starts later if you hold down jump
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            //if(rb.velocity.y < 0){
            rb.gravityScale = gravity * fallMultiplier;
            //}
            //else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
                //rb.gravityScale = gravity * (fallMultiplier/1.7f);
            //}
        }
    }

    //Does a dash
    public IEnumerator Dash(){

        if(dashing == false && dashesLeft > 0 && stunned == false){
            dashing = true;
            dash.Play();
            rb.velocity = new Vector2(0,0);
            fallMultiplier = 0;
            yield return new WaitForSeconds(0.05f);
            rb.velocity = new Vector2(0,0);
            if(direction == new Vector2(0,0)) direction = new Vector2(facingRight ? 1 : -1 ,0);
            rb.AddForce(dashSpeed * direction.normalized, ForceMode2D.Impulse);
            direction = new Vector2(0,0);
            yield return new WaitForSeconds(0.2f);
            fallMultiplier = staticFallMultiplier;
            dash.Stop();
            dashesLeft -= 1;
            dashing = false;
            GetComponent<Punch>().PunchAction(4);
            //stunned = true;
        }
    }

    //Starts counting down till your no longer stunned
    IEnumerator stunnedTimer() {
        stunRecovery = true;
        yield return new WaitForSeconds(howLongYouAreStunned);
        stunned = false;
        stunRecovery = false;
    }
    private void OnDrawGizmos(){
        //Shows me where my ground check raycasts point
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * distanceToGround);
        Gizmos.DrawLine(transform.position + new Vector3(-colliderOffset.x,colliderOffset.y,0), transform.position +  new Vector3(-colliderOffset.x,colliderOffset.y,0) + Vector3.down * distanceToGround);
        Gizmos.DrawWireSphere(footCenter.position, headBopRadius);
    }
}
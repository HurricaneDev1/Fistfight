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
        [SerializeField]private float headBopAmount;

        [SerializeField]private float howLongYouAreStunned = 1f;
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
        RaycastHit2D guy = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, distanceToGround, playerLayer);
        if(guy && guy.rigidbody.gameObject != gameObject){
            Debug.Log(guy.rigidbody.gameObject.name);
            rb.AddForce(Vector2.up * headBopAmount, ForceMode2D.Impulse);
            guy.rigidbody.AddForce(Vector2.down * headBopAmount, ForceMode2D.Impulse);
        }
        

        //Gets directional input from the player
        //direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
       // direction = playerInput.Player.Movement.ReadValue<Vector2>();
  
        // if(playerInput.Player.Jump.WasPressedThisFrame()){
        //     //Starts a jump delay so you don't have to time your jumps perfectly to string them
        //     jumpTimer = Time.time + jumpDelay;
        // }

        // if(playerInput.Player.Dash.WasPressedThisFrame()){
        //     StartCoroutine(Dash());
        // }

    }

    void FixedUpdate()
    {
        //Moves character and checks to see if it should change the gravity and drag
        
            //moveCharacter();
       moveCharacter();
        if(stunned == true && stunRecovery == false) {
            StartCoroutine(stunnedTimer());
        }

        modifyPhysics();

        if(Mathf.Abs(rb.velocity.y) > maxVerticalSpeed){
            rb.velocity = new Vector2(rb.velocity.x , Mathf.Sign(rb.velocity.y) * maxVerticalSpeed);
        }

        //If the jumptimer is still active since you inputted and you still have your coyote time/on the ground you jump
        if(jumpTimer > Time.time && (onGround || coyoteTimer > Time.time)){
            Jump();
        }
    }

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

    public void StartJump(){
        //if(jumpTimer < Time.time){
            jumpTimer = Time.time + jumpDelay;
        //}
    }

    void Jump()
    {
        //Makes the character jump
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        //Resets coyote and jump timer to prevent exploits
        jumpTimer = 0;
        coyoteTimer = 0;

    }

    void Flip(){
        //Changes the facing right variable to its opposite than flips the player
        facingRight = !facingRight;
        player.transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

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

    public IEnumerator Dash(){

        if(dashing == false && dashesLeft > 0 && stunned == false){
            coyoteTimer = 0;
            dashing = true;
            rb.velocity = new Vector2(0,0);
            fallMultiplier = 0;
            yield return new WaitForSeconds(0.05f);
            rb.velocity = new Vector2(0,0);
            if(direction == new Vector2(0,0)) direction = new Vector2(facingRight ? 1 : -1 ,0);
            rb.AddForce(dashSpeed * direction.normalized, ForceMode2D.Impulse);
            direction = new Vector2(0,0);
            yield return new WaitForSeconds(0.2f);
            fallMultiplier = staticFallMultiplier;
            dashesLeft -= 1;
            dashing = false;
            GetComponent<Punch>().PunchAction(4);
            //stunned = true;
        }
    }

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
    }
}
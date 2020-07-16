using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;
    private Animator animator;
    private SpriteRenderer spriterender;
    private myGameManager manager;

    //public Transform groundCheckPoint;
    public Transform groundCheckA, groundCheckB, wallCheckR1, wallCheckR2, wallCheckL1, wallCheckL2;
    public LayerMask ground;

    //Camera
    public bool adaptiveCamera;
    public Transform camTarget;
    public float aheadAmount, aheadSpeed;

    //Jumping
    public bool enableWallJumping = false;
    public float hangTime = 0.2f;
    private float hangCounter;

    public float jumpBufferLength = 0.2f;
    float jumpBufferCounter;
    

    private int jumpState;
    //private int prevWallDir;// thisWallDir;

    //Sliding
    private float slideBufferCounter;
    private bool walkState;
    private float slideTime;
    public float slideSpeedBoost;
    public float maxSlideTime;
    

    //Movement speed and jump force and funny things.
    public float movementSpeed = 1;
    public float jumpForce = 1;
    public float wallJumpHeight = 1.2f;
    public float wallJumpSpeed = 1.5f;
    public float wallJumpCountModifier = 1;
    private float wallJumpCount = 1;

    public float doubleJumpForce;
    public float doubleJumpSpeedImpact;
    public float doubleJumpMaxDownwardVelocity;

    public float airDashJumpForce;
    public float airDashSpeed;
    bool doubleJumpAvailable;
    public float maxAirdashTime;
    float airdashTime;
    bool airDashing;
    bool airDashAvailable;

    //Gravity values
    public float jumpingGrav = 0.6f;
    public float quickFallGrav = 1.2f;
    private float standardGrav;


    public bool isGrounded, wallTouch, wallToRight;

    public float vertical;

    float horizontal;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        manager = GameObject.Find("Manager").GetComponent<myGameManager>();
        standardGrav = myRigidbody.gravityScale;
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        isGrounded = Physics2D.OverlapArea(groundCheckA.position, groundCheckB.position, ground);

        if (!isGrounded)
        {
            if (enableWallJumping == true) //Turn wall jumping on and off
            {
              
                if (Physics2D.OverlapArea(wallCheckR1.position, wallCheckR2.position, ground) == true) //Checking if wall is to the right
                {
                    jumpState = 2;
                    wallToRight = true;
                    wallTouch = true;
                    spriterender.flipX = false;
                }
                else if (Physics2D.OverlapArea(wallCheckL1.position, wallCheckL2.position, ground) == true) //Checking if wall is to the left
                {
                    jumpState = 2;
                    wallToRight = false;
                    wallTouch = true;
                    spriterender.flipX = true;
                }
                else wallTouch = false;

            }

        }
        else //Everything we need to do when the player is grounded. Resetting things and such, for example.
        {
            jumpState = 1;
            wallJumpCount = 1;
            doubleJumpAvailable = true;
            airDashAvailable = true;
            //prevWallDir = 0;
            
        }

        //Hangtime
        if (isGrounded || wallTouch)
        {
            hangCounter = 0;
        }
        else
        {
            hangCounter += Time.deltaTime;
        }

        jump();
        slide();

        if (adaptiveCamera)
        {
            //Moving Camera
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"), aheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
            }
            else
            {
                camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, 0, aheadSpeed / 2 * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
            }
        }

        //Limiting max falling speed
        if (myRigidbody.velocity.y < -20) myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -20f);

        animationHandler();


    }


    void FixedUpdate()
    { 
        horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
        
    }

    

    void HandleMovement(float horizontal)
    {
        if (walkState == true)
        {
            myRigidbody.velocity /= new Vector2(1.1f, 1);
            if (Mathf.Abs(myRigidbody.velocity.x) <= movementSpeed)
            {
                myRigidbody.velocity += new Vector2(horizontal * movementSpeed * Time.deltaTime, 0); //x= -1, y = 0;
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(myRigidbody.velocity.x) < 1)
            {
                myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
            }
            
        }
    }


    void jump()
    {
        //Jump Buffer
        if (Input.GetButtonDown("Jump"))
        {
            if (!isGrounded && !wallTouch && doubleJumpAvailable && myRigidbody.velocity.y > doubleJumpMaxDownwardVelocity && hangCounter >= hangTime)//Check if we are in the air and can do a double jump. If not, we do normal jumping
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * doubleJumpSpeedImpact, jumpForce*doubleJumpForce);
                doubleJumpAvailable = false; //Disable it after one.
                animator.Play("DoubleJump");
            }
            else
            {
                jumpBufferCounter = jumpBufferLength; //Whenever we press the jump button, our buffer counter is set equal to the buffer length
            }
            walkState = true;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime; //The counter is then reduced over time, and if we reach a state where we can jump before it goes under 0, like when we just land, we jump again.
        }


        //Applying Jump. Need a bunch of if statements in here to determine what kind of jump the character is doing. Unless we do switch statement, and change your state somewhere else!
        if (jumpBufferCounter >= 0 && hangCounter <= hangTime)
        {
            slideTime = 0;
            hangCounter = hangTime;
            jumpBufferCounter = 0;
            myRigidbody.gravityScale = jumpingGrav;

            switch (jumpState)
            {
                //Normal Jump
                case 1:
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                    break;

                //Wall Jump
                case 2:
                    float wallJumpDirection;
                    if (wallToRight)
                    {
                        wallJumpDirection = wallJumpSpeed * -1; 
                        //thisWallDir = 1;
                    }
                    else
                    {
                        wallJumpDirection = wallJumpSpeed;
                        //thisWallDir = 2;
                    }
                    myRigidbody.velocity = new Vector2(jumpForce * wallJumpDirection, jumpForce * wallJumpHeight * 1 / wallJumpCount);
                    /*if (thisWallDir != prevWallDir)
                    {
                        myRigidbody.velocity = new Vector2(jumpForce * wallJumpDirection, jumpForce * wallJumpHeight);
                    }
                    prevWallDir = thisWallDir;*/
                    wallJumpCount += wallJumpCountModifier;
                    break;

            }
            
        }

        if (Input.GetButtonUp("Jump") && myRigidbody.velocity.y > 0) //Making it so that when you release the jump button, you don't jump as high as if you held it.
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f);
        }

        if (enableWallJumping)
        {
            if (myRigidbody.velocity.y < 0 && !wallTouch) //Making it so that when you are against a wall, you don't fall as quick? Depends on standard grav.
            {

                myRigidbody.gravityScale = standardGrav;

            }
        }

        if (Input.GetAxis("Vertical") < 0) //Making it so that if you hold down, you fall quicker.
        {
            myRigidbody.gravityScale = quickFallGrav;
        }

    }


    void slide()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (!isGrounded && !wallTouch && airDashAvailable) //Air dash! Consider removing the wall touch parameter, probably not using it ever again.
            {
                if (horizontal < 0 || spriterender.flipX == true && horizontal == 0) myRigidbody.velocity = new Vector2(-airDashSpeed, jumpForce * airDashJumpForce);
                else myRigidbody.velocity = new Vector2(airDashSpeed, jumpForce * airDashJumpForce);
                airDashAvailable = false;
                walkState = false;
                animator.Play("AirDash");

            }
            else {
                slideBufferCounter = jumpBufferLength;
            }
        }
        else
        {
            slideBufferCounter -= Time.deltaTime;
        }

        if (slideBufferCounter >=0 && isGrounded && walkState == true) //Normal Slide!
        {
            walkState = false;
            if (horizontal < 0 || spriterender.flipX == true && horizontal == 0) myRigidbody.velocity = new Vector2(-slideSpeedBoost, myRigidbody.velocity.y);
            else  myRigidbody.velocity = new Vector2(slideSpeedBoost, myRigidbody.velocity.y);

        }

        if (walkState == false)
        {
            slideTime += 1 * Time.deltaTime;
            if(slideTime >= maxSlideTime || Mathf.Abs(myRigidbody.velocity.x) == 0)
            {
                walkState = true;
                slideTime = 0;
            }
        }

    }



    void animationHandler()
    {
        //Animation
        //Running
        animator.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
        animator.SetFloat("yVelocity", myRigidbody.velocity.y);

        if (myRigidbody.velocity.x > 0)
        {
            spriterender.flipX = false;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            spriterender.flipX = true;
        }

        //Jumping
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("WallTouch", wallTouch);
        animator.SetBool("SlideOrNot", walkState);
    }
}
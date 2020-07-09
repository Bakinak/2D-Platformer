using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;
    private Animator animator;
    private SpriteRenderer spriterender;

    //public Transform groundCheckPoint;
    public Transform groundCheckA, groundCheckB, wallCheckA, wallCheckB;
    public LayerMask ground;

    //Camera
    public bool adaptiveCamera;
    public Transform camTarget;
    public float aheadAmount, aheadSpeed;

    //Jumping
    public float hangTime = 0.2f;
    private float hangCounter;

    public float jumpBufferLength = 0.2f;
    private float jumpBufferCounter;
    

    private int jumpState;
    private int prevWallDir;// thisWallDir;

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

    //Gravity values
    public float jumpingGrav = 0.6f;
    public float quickFallGrav = 1.2f;
    private float standardGrav;


    public bool isGrounded, wallTouch;

    public float vertical;

    float horizontal;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();

        standardGrav = myRigidbody.gravityScale;
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        isGrounded = Physics2D.OverlapArea(groundCheckA.position, groundCheckB.position, ground);

        if (!isGrounded)
        {
            wallTouch = Physics2D.OverlapArea(wallCheckA.position, wallCheckB.position, ground);
            if (wallTouch)
            {
                jumpState = 2;
            }
            walkState = true;
        }
        else
        {
            jumpState = 1;
            prevWallDir = 0;
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

        if (isGrounded)
        {
            wallJumpCount = 1;
        }

        jump();
        airdash();
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

            if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(myRigidbody.velocity.x) < 0.5)
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
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
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
                    myRigidbody.velocity += new Vector2(0, jumpForce);
                    break;

                //Wall Jump
                case 2:
                    float wallJumpDirection;
                    if (spriterender.flipX)
                    {
                        wallJumpDirection = wallJumpSpeed;
                        //thisWallDir = 1;
                    }
                    else
                    {
                        wallJumpDirection = wallJumpSpeed * -1;
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

        if (Input.GetButtonUp("Jump") && myRigidbody.velocity.y > 0)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f);
        }

        if (myRigidbody.velocity.y < 0 && !wallTouch)
        {

            myRigidbody.gravityScale = standardGrav;
            
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            myRigidbody.gravityScale = quickFallGrav;
        }

    }


    void slide()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            slideBufferCounter = jumpBufferLength;
        }
        else
        {
            slideBufferCounter -= Time.deltaTime;
        }

        if (slideBufferCounter >=0 && isGrounded && walkState == true)
        {
            walkState = false;
            if (spriterender.flipX == true)
            {
                myRigidbody.velocity -= new Vector2(slideSpeedBoost, 0f);
            }
            else
            {
                myRigidbody.velocity += new Vector2(slideSpeedBoost, 0f);
            }

            Debug.Log("sliding!");
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
    

    void airdash()
    {
        if (Input.GetButtonDown("Fire1") && !isGrounded)
        {
            Debug.Log("dashing!");
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
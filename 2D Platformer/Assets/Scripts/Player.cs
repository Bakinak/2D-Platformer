using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public bool inControl = true;

    private Rigidbody2D myRigidbody;
    private BoxCollider2D myBoxCollider;
    private Animator animator;
    private SpriteRenderer spriterender;
    private myGameManager manager;

    //public Transform groundCheckPoint;
    public Transform groundCheckA, groundCheckB;
    public LayerMask ground;

    //Camera
    /*public bool adaptiveCamera;
    public Transform camTarget;
    public float aheadAmount, aheadSpeed;*/

    //Jumping
    public float hangTime = 0.1f;
    private float hangCounter;

    public float jumpBufferLength = 0.1f;
    float jumpBufferCounter;
    

    private int jumpState;
    //private int prevWallDir;// thisWallDir;

    //Sliding
    private float slideBufferCounter;
    private bool walkState;
    private float slideTime;
    public float slideSpeedBoost = 12;
    public float maxSlideTime = 0.5f;
    

    //Movement speed and jump force and funny things.
    public float movementSpeed = 36;
    public float jumpForce = 14;

    public float doubleJumpForce = 1;
    public float doubleJumpSpeedImpact;
    public float doubleJumpMaxDownwardVelocity = -10;

    public float airDashJumpForce = 0.4f;
    public float airDashSpeed = 18;
    public bool doubleJumpAvailable;
    public float maxAirdashTime = 0;
    float airdashTime;
    bool airDashing;
    public bool airDashAvailable;

    //Gravity values
    public float jumpingGrav = 1;
    public float quickFallGrav = 1.4f;
    private float standardGrav;

    //Attacks
    public float action1Cooldown;
    float action1Time;
    public float action2Cooldown;
    float action2Time;
    float action1Buffer;
    float action2Buffer;

    //Invincibility and spriteflicker
    bool invincible;
    float invincibilityTime = 1f; //How long the player can't take damage after having taken damage. Safety time, so your health isn't just drained.
    float timeInvincible;
    float blinkTime = 0.07f; //Lower this to make the character flicker faster when hit.
    float timeBlinked;


    public bool isGrounded;
    public float vertical;

    float horizontal;

    public GameObject deathAnim;

    void Start()
    {
        putThisInStart();
    }

    void Update()
    {
        putThisInUpdate();  
    }


    void FixedUpdate()
    {
        putThisInFixedUpdate();
    }

    public void putThisInStart()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriterender = GetComponent<SpriteRenderer>();
        manager = GameObject.Find("Manager").GetComponent<myGameManager>();
        standardGrav = myRigidbody.gravityScale;
    }

    public void putThisInUpdate()
    {
        vertical = Input.GetAxis("Vertical");

        isGrounded = Physics2D.OverlapArea(groundCheckA.position, groundCheckB.position, ground);

        if (!isGrounded)
        {

        }
        else //Everything we need to do when the player is grounded. Resetting things and such, for example.
        {
            resettingDashAndJump();
        }

        //Hangtime
        if (isGrounded)
        {
            hangCounter = 0;
        }
        else
        {
            hangCounter += Time.deltaTime;
        }

        //Functions and stuff
        if (inControl)
        {
            jump();
            slide();
            attacks();
        }

        /*if (adaptiveCamera)
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
        }*/

        //Limiting max falling speed
        if (myRigidbody.velocity.y < -20) myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -20f);

        //Incinvibility. Also sprite flickering.
        if (invincible)
        {
            timeInvincible -= Time.deltaTime;
            if (timeInvincible <= 0)
            {
                invincible = false;
                spriterender.enabled = true;
            }
            else
            {
                timeBlinked -= Time.deltaTime;
                if(timeBlinked <= 0)
                {
                    if (spriterender.enabled == true) spriterender.enabled = false;
                    else spriterender.enabled = true;
                    timeBlinked = blinkTime;
                }
            }
        }

        animationHandler();
    }

    public void putThisInFixedUpdate()
    {

        horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
       
    }

    void HandleMovement(float horizontal)
    {
        if (walkState == true)
        {
            jumpState = 1;
            myRigidbody.velocity /= new Vector2(1.1f, 1);
            if(inControl)
            {
                if (Mathf.Abs(myRigidbody.velocity.x) <= movementSpeed)
                {
                    myRigidbody.velocity += new Vector2(horizontal * movementSpeed * Time.deltaTime, 0); //x= -1, y = 0;
                }
            }
            if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(myRigidbody.velocity.x) < 3)
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
            if (!isGrounded && doubleJumpAvailable && myRigidbody.velocity.y > doubleJumpMaxDownwardVelocity && hangCounter >= hangTime)//Check if we are in the air and can do a double jump. If not, we do normal jumping
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

                case 2:
                    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x * 1.5f, jumpForce * 0.9f);
                    jumpState = 1;
                    break;

            }
            
        }

        if (Input.GetButtonUp("Jump") && myRigidbody.velocity.y > 0) //Making it so that when you release the jump button, you don't jump as high as if you held it.
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f);
        }


        if (Input.GetAxis("Vertical") < 0) //Making it so that if you hold down, you fall quicker.
        {
            myRigidbody.gravityScale = quickFallGrav;
        }

    }


    void slide()
    {
        if(Input.GetButtonDown("Fire1"))
        { //Change these things to airdashAvailable if I again want to split the two into both being available in a single jump.
            if (!isGrounded && doubleJumpAvailable) //Air dash! Consider removing the wall touch parameter, probably not using it ever again.
            {
                if (horizontal < 0 || spriterender.flipX == true && horizontal == 0) myRigidbody.velocity = new Vector2(-airDashSpeed, jumpForce * airDashJumpForce);
                else myRigidbody.velocity = new Vector2(airDashSpeed, jumpForce * airDashJumpForce);
                doubleJumpAvailable = false;
                walkState = false;
                slideTime = 0;
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
            slideTime = 0;
            if (horizontal < 0 || spriterender.flipX == true && horizontal == 0) myRigidbody.velocity = new Vector2(-slideSpeedBoost, myRigidbody.velocity.y);
            else  myRigidbody.velocity = new Vector2(slideSpeedBoost, myRigidbody.velocity.y);

        }

        if (walkState == false)
        {
            jumpState = 2;
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
        animator.SetBool("SlideOrNot", walkState);
    }

    public void resettingDashAndJump()
    {
        //jumpState = 1;
        doubleJumpAvailable = true;
        //airDashAvailable = true;
    }


    void attacks()
    {
        if (Input.GetButtonDown("Fire2")) action1Buffer = jumpBufferLength;

        if (Input.GetButtonDown("Fire3")) action2Buffer = jumpBufferLength;

        if (walkState || isGrounded)
        {
            if (action1Buffer > 0 && action1Time <= 0)
            {
                action1(spriterender.flipX);
                action1Time = action1Cooldown;
                walkState = true;
                animator.Play("Attack");
            }

            if (action2Buffer > 0 && action2Time <= 0)
            {
                action2(spriterender.flipX);
                action2Time = action2Cooldown;
                walkState = true;
                animator.Play("SpecialAttack");
            }
        }

        if (action1Buffer >= 0) action1Buffer -= Time.deltaTime;
        if (action2Buffer >= 0) action2Buffer -= Time.deltaTime;
        if (action1Time > 0) action1Time -= Time.deltaTime;
        if (action2Time > 0) action2Time -= Time.deltaTime;

    }

    public void takeDamage(int damage)//Add invincibility frames.
    {
        if (!invincible)
        {
            
            invincible = true;
            timeInvincible = invincibilityTime;
            animator.Play("Hurt");
            walkState = false;
            slideTime = maxSlideTime - 0.2f;
            if (spriterender.flipX == true) myRigidbody.velocity = new Vector2(3, 0);
            else myRigidbody.velocity = new Vector2(-3, 0);
            manager.changeHealth(-damage);
        }
    }

    public void gainHealth(int heal) //Play healing sound also. Unless that is done in the manager every time health ticks up.
    {
        manager.changeHealth(heal);
    }

    //Called from manager when player dies. Should reset some values, like cooldown, invincibility, sliding, and so on.
    public void died()
    {
        slideTime = maxSlideTime;
        timeInvincible = 0;
        action1Time = 0;
        action2Time = 0;
    }

    public virtual void action1(bool direction) //Direction used to know which way attack goes... obviously. But the sprite renderer is not available to each character, so it is sent here.
    {

    }

    public virtual void action2(bool direction)
    {

    }
}
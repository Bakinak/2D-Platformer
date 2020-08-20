using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bakeryBoss : BossClass
{   
    public float jumpForce;
    int actionChooser;
    bool attacking, sucking;
    bool oneTimeThings, OneTimeThing2, OneTimeThing3;
    bool attackChosen;
    float coolDownTime;
    float timePassed;
    float otherCoolDownTime = 2f;
    float otherTimePassed;
    
    int lastAttack = 4;
    
    public float suckForce = 2;
    public Transform groundChecker, shockWaveSpawnSpot;
    public GameObject shockWave;
    bool grounded;
    public GameObject skippzzz;
    
    AudioSource myAudio;
    public AudioClip jumpSound, landSound, suckingSound, walkingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        callOnStart();
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bossActive)
        {
            callOnUpdate();
            
            grounded = Physics2D.OverlapCircle(groundChecker.position, 0.5f, LayerMask.GetMask("Ground"));
            
            animationHandler();
            otherTimePassed += Time.deltaTime;
            if(otherTimePassed > otherCoolDownTime){
                
            
            
                if(!attackChosen) rollDice();
                
                if(attacking){
                    switch(actionChooser){
                        case 0:
                            walkAttack();
                            break; 
                        case 1:
                            jumpAttack();
                            break;
                        
                        case 2:
                            suckAttack();
                            break;
                    }
                }
            }
            
        }
    }

    void chooseDirection()
    {   
        if(GameObject.FindGameObjectWithTag("Player") != null){   
            if(GameObject.FindGameObjectWithTag("Player").transform.position.x < transform.position.x){
                movementSpeed = originalMoveSpeed * -1;
            } else movementSpeed = originalMoveSpeed;
        }
    }

    void rollDice()
    {
        actionChooser = Random.Range(0, 3);
        if(actionChooser != lastAttack){
            attackChosen = true;
            attacking = true;
            lastAttack = actionChooser;
        } else{
            rollDice();
        }
        
    }
    
    void walkAttack(){
        if(!oneTimeThings){
            chooseDirection();
            oneTimeThings = true;
            timePassed = 0;
            coolDownTime = 1.5f;
            myAudio.clip = walkingSound;
            myAudio.loop = true;
            myAudio.Play();
            damageOnTouch = 3;
        }        
        
        myrigidbody.velocity = new Vector2(movementSpeed*2.5f, myrigidbody.velocity.y);        
        
        timePassed += Time.deltaTime;
        
        if(timePassed >= coolDownTime || Physics2D.OverlapCircle(new Vector2(transform.position.x + movementSpeed/4, transform.position.y + 2), 2f, LayerMask.GetMask("Ground"))){
            attackOver();
            myAudio.Stop();
            myAudio.loop = false;
        }
        
        
    }
    
    void jumpAttack(){
        
        timePassed += Time.deltaTime;
        
        if(!oneTimeThings){
            animator.Play("ChargeJump");
            oneTimeThings = true;
        }
        
    
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("JumpTrigger")){
            if(!OneTimeThing2){
                damageOnTouch = 3;
                OneTimeThing2 = true;
                chooseDirection();
                myrigidbody.velocity = new Vector2(movementSpeed * 2, jumpForce);
                playSound(jumpSound);
            }
        }
                
        if(!OneTimeThing3 && animator.GetCurrentAnimatorStateInfo(0).IsName("Land")){
            attackOver();
            Instantiate(shockWave, shockWaveSpawnSpot.position, Quaternion.Euler(0, 180, 0));
            Instantiate(shockWave, shockWaveSpawnSpot.position, Quaternion.Euler(0, 0, 0));
            playSound(landSound);
            }
        
        if(timePassed > 4) attackOver(); //fail safe to get boss out of being stuck. Hopefully
    }
    
    void suckAttack(){
        if(!oneTimeThings){
            animator.Play("SuckCharge");
            oneTimeThings = true;
            sucking = true;
            coolDownTime = 3;
            myAudio.clip = suckingSound;
            myAudio.loop = true;
            myAudio.Play();
        }
        
        if(sucking){          
            timePassed += Time.deltaTime;
            
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if(player != null){
                if(player.transform.position.x >= transform.position.x){
                    player.transform.position += new Vector3(-suckForce *Time.deltaTime, 0, 0);
                } else player.transform.position += new Vector3(suckForce *Time.deltaTime, 0, 0);
            }
            if(timePassed >= coolDownTime){
                sucking = false;
                OneTimeThing2 = true;
            }
        }
        
        if(OneTimeThing2){
            attackOver();
            myAudio.Stop();
            myAudio.loop = false;
        }
        
    }
    
       
    void attackOver(){
        timePassed = 0;
        attacking = false;
        attackChosen = false;
        oneTimeThings = false;
        OneTimeThing2 = false;
        OneTimeThing3 = false;
        sucking = false;
        otherTimePassed = 0;
        damageOnTouch = 2;
    }
    
    void animationHandler(){
        animator.SetFloat("movementSpeed", Mathf.Abs(myrigidbody.velocity.x));
        animator.SetBool("grounded", grounded);
        animator.SetFloat("yVelocity", myrigidbody.velocity.y);
        animator.SetBool("sucking", sucking);
    }
    
    public override void respawn(){
        base.respawn();
        attackOver();
        actionChooser = 4;
        animator.Play("Idle");
        myAudio.Stop();
        myAudio.loop = false;
    }
    
    public override void death(){
        GameObject hello = Instantiate(skippzzz, transform);
        hello.transform.parent = null;
        hello.SetActive(true);       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skippzzz : Player
{
    public GameObject[] projectiles;
    public AudioClip throwSound;
    public AudioClip suckingSound;
    public AudioClip eatSound;
    AudioSource myAudio;
    bool sucking;
    float suckingCooldown = 0.5f;
    float suckTimer;
    int nextProjectile;
    // Start is called before the first frame update
    void Start()
    {
        putThisInStart();
        myAudio = GetComponent<AudioSource>();
        myAudio.clip = suckingSound;
    }

    // Update is called once per frame
    void Update()
    {
        putThisInUpdate();
        
        if(Input.GetButtonUp("Fire3")){
            suckTimer = suckingCooldown;
        }
        if(suckTimer >= 0) suckTimer -=Time.deltaTime;
        
    }
    
    void FixedUpdate(){
        if(!sucking){
        putThisInFixedUpdate();
        }
    }
        
    public override void action1(bool direction){
        base.action1(direction);
        
        if(direction){
            Instantiate(projectiles[nextProjectile], transform.position, Quaternion.Euler(0, 180, 0));
        }
        else{
            Instantiate(projectiles[nextProjectile], transform.position, Quaternion.Euler(0, 0, 0));
        }
        nextProjectile +=1;
        if(nextProjectile > 2) nextProjectile = 0; 
        playSound(throwSound);
    }
    
    public override void action2(bool direction){
        if(Input.GetButton("Fire3") && suckTimer < 0){
            if(myAudio.isPlaying == false) myAudio.Play();
            base.action2(direction);
            sucking = true;
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);         
        } else{
            if(sucking) sucking = false;
            myAudio.Stop();         
        }
    }
    
    public override void takeDamage(int damage, int damageType){
        if(sucking && damageType == 1){
            playSound(eatSound);
            //Charge up some bar or something.
        } else{
            suckTimer = suckingCooldown;
            base.takeDamage(damage, damageType);         
        }
    }
    
}

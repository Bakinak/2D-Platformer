using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDashReset : MonoBehaviour
{
    float jumpBoost = 6;
    public float respawnTime;
    bool popped;
    bool theSecondPoppening;
    Animator animator;
    float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //PROBLEM: THE ANIMATION IS STILL PLAYING IDLE WHEN WE POP, AND IT RUNS THIS CODE ONCE IN UPDATE BEFORE PLAYING ANIMATION, WHICH MEANS SPRITE RENDERER IS TURNED OFF IMMEDIATELY, AND NOT AFTER ANIMATION
        if (popped == true && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if(timePassed < respawnTime)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            
            timePassed -= Time.deltaTime;
            if (timePassed < 0)
            {
                popped = false;
                toggleStuff(true);
            }


        }
        
    }

     void action(GameObject target)
    {
        if (!target.GetComponent<Player>().doubleJumpAvailable || !target.GetComponent<Player>().airDashAvailable)
        {
            timePassed = respawnTime;
            animator.SetTrigger("Pop");
            popped = true;

            GetComponent<BoxCollider2D>().enabled = false;
            target.GetComponent<Player>().resettingDashAndJump();
            if (target.GetComponent<Rigidbody2D>().velocity.y < jumpBoost) target.GetComponent<Rigidbody2D>().velocity = new Vector2(target.GetComponent<Rigidbody2D>().velocity.x, jumpBoost);
            else target.GetComponent<Rigidbody2D>().velocity = new Vector2(target.GetComponent<Rigidbody2D>().velocity.x, target.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    void toggleStuff(bool state)
    {
        GetComponent<BoxCollider2D>().enabled = state;
        GetComponent<SpriteRenderer>().enabled = state;
        //GetComponent<Animator>().enabled = state;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            action(collision.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDashReset : PickUpClass
{
    public float jumpBoost = 2;
    public float respawnTime;
    bool popped;
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
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).shortNameHash.ToString());
            GetComponent<SpriteRenderer>().enabled = false;
            timePassed -= Time.deltaTime;
            if(timePassed < 0)
            {
                popped = false;
                toggleStuff(true);
            }
        }
        
    }

    public override void action(GameObject target)
    {
        timePassed = respawnTime;
        animator.SetTrigger("Pop");
        popped = true;

        GetComponent<BoxCollider2D>().enabled = false;
        target.GetComponent<Player>().resettingDashAndJump();
        target.GetComponent<Rigidbody2D>().velocity = new Vector2(target.GetComponent<Rigidbody2D>().velocity.x, jumpBoost);

    }

    void toggleStuff(bool state)
    {
        GetComponent<BoxCollider2D>().enabled = state;
        GetComponent<SpriteRenderer>().enabled = state;
        //GetComponent<Animator>().enabled = state;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialBoss : BossClass
{
    float checkDistance = 1;

    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        callOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (bossActive)
        {
            callOnUpdate();
            moveBackAndForth();
            animationHandler();

            if(hit && Physics2D.OverlapArea(transform.position, new Vector2(transform.position.x, transform.position.y - 0.8f), LayerMask.GetMask("Ground")))
            {
                hit = false;
            }
        }
    }

    void moveBackAndForth()
    {
        if (!hit)
        {
            myrigidbody.velocity = new Vector2(movementSpeed, myrigidbody.velocity.y);
        }

        if(Physics2D.OverlapArea(transform.position, new Vector2(transform.position.x+checkDistance, transform.position.y +0.5f), LayerMask.GetMask("Ground")))
        {
            movementSpeed *= -1;
            checkDistance *= -1;
            flipRender();
        }


    }

    void flipRender()
    {
        if (movementSpeed < 0) spriterender.flipX = false;
        else spriterender.flipX = true;
    }

    void animationHandler()
    {
        animator.SetFloat("velocityX", Mathf.Abs(myrigidbody.velocity.x));
        animator.SetFloat("velocityY", Mathf.Abs(myrigidbody.velocity.y));
    }

    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        hit = true;

        myrigidbody.velocity = new Vector2(myrigidbody.velocity.x, 20);

        if (movementSpeed < 0) movementSpeed -= 1f;
        else movementSpeed += 1f;

    }

}

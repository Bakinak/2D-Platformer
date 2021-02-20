using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoppefjende : EnemyPatrol
{
    public float jumpForce = 5;
    public float coolDown = 3;
    float timePassed;
    bool shot;
    bool charging;
    public LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        callOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyActive)
        {
            callOnUpdate();
            patrol();
        }

    }

    public override void patrol()
    {
        if (transform.position.x <= pos1.position.x && movementSpeed < 0 || transform.position.x >= pos2.position.x && movementSpeed > 0)
        {
            movementSpeed *= -1;
            flipRender();
        }

        jumpAttack();

    }

    void jumpAttack()
    {
        if (timePassed < coolDown)
        {
            timePassed += Time.deltaTime;
        }
        else
        {
            //Attack!
            if (!charging)
            {
                charging = true;
                animator.Play("Charge");
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump!"))
        {

            if (!shot)
            {
                shot = true;
                //Spawn projectile, and send it right direction.

                jump();
            }

            if (myrigidbody.velocity.y <= 0 && checkGround())
            {
                animator.Play("Idle");
                smallReset();
            }

        }
    }

    void jump()
    {
        myrigidbody.velocity = new Vector2(movementSpeed, jumpForce);
    }

    bool checkGround()
    {
        if(Physics2D.OverlapCircle(transform.position - new Vector3(0, 1, 0), 1f, ground))
        {
            Debug.Log("Hey");
            return true;
        }
        return false;
    }

    IEnumerator waitForAnim(float time)
    {
        yield return new WaitForSeconds(time);
        smallReset();
    }


    public override void respawn()
    {
        base.respawn();
        smallReset();
    }

    void smallReset()
    {
        shot = false;
        charging = false;
        timePassed = 0;
    }


}

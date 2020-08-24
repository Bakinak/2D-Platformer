using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muffinEnemy : EnemyPatrol
{
    public bool blocking;
    bool chargingAttack;
    bool punched;
    float checkDistance = 1;

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
            checkForPlayer();
            animationHandler();


            if (chargingAttack)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punched"))
                {
                    if (spriterender.flipX) myrigidbody.velocity = new Vector2(2.5f, 0);
                    else myrigidbody.velocity = new Vector2(-2.5f, 0);

                    if (!punched)
                    {
                        punched = true;
                        Collider2D target = Physics2D.OverlapCircle(new Vector2(transform.position.x + checkDistance, transform.position.y), 0.6f, LayerMask.GetMask("Player"));
                        if (target != null) target.GetComponent<Player>().takeDamage(4, 0);
                        StartCoroutine(waitforAnim(0.3f));
                    }
                }
            }

        }
    }

    public override void takeDamage(int damage)
    {
        if (!blocking) base.takeDamage(damage);
    }

    public override void flipRender()
    {
        base.flipRender();
        checkDistance *= -1;
    }

    void checkForPlayer()
    {
        if (!chargingAttack)
        {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x + checkDistance, transform.position.y), 1, LayerMask.GetMask("Player")))
            {
                movementSpeed = 0;
                chargingAttack = true;
                animator.Play("Charge");
            }
        }
    }

    void animationHandler()
    {
        animator.SetFloat("velocity", Mathf.Abs(myrigidbody.velocity.x));
    }

    public override void respawn()
    {
        base.respawn();
        chargingAttack = false;
        checkDistance = 1;
        blocking = false;
        animator.Play("Idle");
    }

    IEnumerator waitforAnim(float time)
    {
        yield return new WaitForSeconds(time);
        chargingAttack = false;
        punched = false;
        if(spriterender.flipX) movementSpeed = originalMoveSpeed;
        else movementSpeed = originalMoveSpeed*-1;

    }

}

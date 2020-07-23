using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    private SpriteRenderer spriterender;

    public int health;
    public int damageOnTouch = 2;

    public float movementSpeed;
    float originalMoveSpeed;
    bool stunned;
    float stunTime = 0.3f;
    float timeStunned;
    float timeBlinked = 0.07f;

    public GameObject deathAnim;

    //Respawning?
    int originalHealth;
    Transform originalTransform;

    // Start is called before the first frame update
    void Start()
    {
        callOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        callOnUpdate();
    }

    //Probably need to create a struct that contains information about the enemy, such as start health and position, so it can be reset of the player dies and loads a checkpoint.
    public void callOnStart()
    {
        originalMoveSpeed = movementSpeed;
        spriterender = GetComponent<SpriteRenderer>();
        originalTransform = transform;
        originalHealth = health;
    }

    public void callOnUpdate()
    {
        if(stunned)
        {
            timeStunned -= Time.deltaTime;
            if (timeStunned <= 0)
            {
                movementSpeed = originalMoveSpeed;
            }
            else if (spriterender.enabled == false && timeStunned <= stunTime - timeBlinked) spriterender.enabled = true; 

            
        }
    }

    public virtual void takeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            if(deathAnim != null) Instantiate(deathAnim, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        else
        {
            stunned = true;
            movementSpeed = 0; //Stunning the enemy. Set back to normal after a while.
            timeStunned = stunTime; //Remember to update stun time here if changed above.
            spriterender.enabled = false;
        }
    }

    public void respawn()
    {
        gameObject.SetActive(true);
        health = originalHealth;
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
        timeStunned = 0;
        movementSpeed = originalMoveSpeed;
        if (gameObject.GetComponent<Rigidbody2D>() != null) gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().takeDamage(damageOnTouch);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : soundClass
{
    public bool enemyActive;

    public SpriteRenderer spriterender;
    public Rigidbody2D myrigidbody;
    public Animator animator;

    public int health;
    public int damageOnTouch = 2;

    public float movementSpeed;
    float originalMoveSpeed;
    bool stunned;
    float stunTime = 0.1f;
    float timeStunned;
    float timeBlinked = 0.07f;

    public GameObject deathAnim;

    LayerMask layer;
    BoxCollider2D myCollider;
    Collider2D playerToDamage;
    //Respawning?
    int originalHealth;
    Vector3 originalPosition;
    Vector3 originalRotation;

    //Sound
    public AudioClip hitSound;

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
    public virtual void callOnStart()
    {
        originalMoveSpeed = movementSpeed;
        spriterender = GetComponent<SpriteRenderer>();
        if (GetComponent<Rigidbody2D>() != null) myrigidbody = GetComponent<Rigidbody2D>();
        if (GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation.eulerAngles;
        originalHealth = health;
        layer = LayerMask.GetMask("Player");
        myCollider = GetComponent<BoxCollider2D>();
    }

    public void callOnUpdate()
    {
        if(stunned)
        {
            timeStunned -= Time.deltaTime;
            if (timeStunned <= 0)
            {
                //movementSpeed = originalMoveSpeed;
            }
            if (spriterender.enabled == false && timeStunned <= stunTime - timeBlinked) spriterender.enabled = true; 

        }

        if (spriterender.enabled == false && timeStunned <= stunTime - timeBlinked) spriterender.enabled = true;

        collisionDamage();
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
            timeStunned = stunTime; 
            spriterender.enabled = false;
            if(hitSound !=null) playSound(hitSound);
        }
    }

    public virtual void respawn()
    {
        gameObject.SetActive(true);
        health = originalHealth;
        transform.position = originalPosition;
        transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z);
        timeStunned = 0;
        movementSpeed = originalMoveSpeed;
        if (gameObject.GetComponent<Rigidbody2D>() != null) gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void collisionDamage() //Checking whether we can damage a player. All enemies must have boxcolliders 2D for this work in its current state.
    {
        playerToDamage = Physics2D.OverlapArea(new Vector2(transform.position.x - myCollider.size.x / 2, transform.position.y - myCollider.size.y / 2), 
            new Vector2(transform.position.x + myCollider.size.x / 2, transform.position.y + myCollider.size.y / 2), layer);
        if (playerToDamage != null) playerToDamage.GetComponent<Player>().takeDamage(damageOnTouch);
    }

}

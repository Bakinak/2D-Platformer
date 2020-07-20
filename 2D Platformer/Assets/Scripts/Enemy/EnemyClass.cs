using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public int health;
    public int damageOnTouch = 2;

    public float movementSpeed;
    float originalMoveSpeed;

    float timeStunned = 0.3f; //The amount of time enemy is stunned when hit. 

    public GameObject deathAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Probably need to create a struct that contains information about the enemy, such as start health and position, so it can be reset of the player dies and loads a checkpoint.
    public void callOnStart()
    {
        deathAnim.SetActive(false);
        originalMoveSpeed = movementSpeed;
    }

    public void callOnUpdate()
    {
        if(movementSpeed != originalMoveSpeed)
        {
            timeStunned -= Time.deltaTime;
            if(timeStunned < 0)
            {
                movementSpeed = originalMoveSpeed;                
            }
        }
    }

    public virtual void takeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            deathAnim.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            movementSpeed = 0; //Stunning the enemy. Set back to normal after a while.
            timeStunned = 0.3f; //Remember to update stun time here if changed above.
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().takeDamage(damageOnTouch);
        }
    }

}

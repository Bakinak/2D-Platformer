using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectileClass : ProjectileClass
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject target = collision.gameObject;
            playerHit(target);
        }
        
    }


    public virtual void playerHit(GameObject target)
    {
        target.GetComponent<Player>().takeDamage(damage);
        expire();
    }

}

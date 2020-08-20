using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : ProjectileClass
{
    BoxCollider2D myCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        collisionDamage();
    }
    
    void collisionDamage() //Checking whether we can damage a player. All enemies must have boxcolliders 2D for this work in its current state.
    {
        Collider2D enemyToDamage = Physics2D.OverlapArea(new Vector2(transform.position.x - myCollider.size.x / 2, transform.position.y - myCollider.size.y / 2), 
            new Vector2(transform.position.x + myCollider.size.x / 2, transform.position.y + myCollider.size.y / 2), LayerMask.GetMask("Enemy"));
        if (enemyToDamage != null) enemyHit(enemyToDamage.gameObject);
    }


    public virtual void enemyHit(GameObject target)
    {
        target.GetComponent<EnemyClass>().takeDamage(damage);
        expire();
    }
}

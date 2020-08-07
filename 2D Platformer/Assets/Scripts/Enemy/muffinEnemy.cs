using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muffinEnemy : EnemyPatrol
{
    public bool blocking;

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

    public override void takeDamage(int damage)
    {
        if (!blocking) base.takeDamage(damage);
    }

}

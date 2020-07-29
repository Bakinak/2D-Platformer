using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClass : EnemyClass
{
    public bool bossActive;
    myGameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This class should probably update the take damage function so it calls manager and updates boss health on UI.
    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        manager.bossTakeDamage(damage);
    }

    public override void callOnStart()
    {
        base.callOnStart();
        manager = GameObject.Find("Manager").GetComponent<myGameManager>();
        manager.boss = gameObject;
    }
}

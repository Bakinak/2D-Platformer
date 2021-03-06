﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mike : Player
{
    public Transform attackPos1, attackPos2, attackPos3, attack2Pos1, attack2Pos2;
    public LayerMask enemyLayer;
    public int damage;
    Collider2D[] enemiesToDamage;
    public GameObject swordSlash;
    float slashDistance = 1.7f;
    public bool flipSlash;

    // Start is called before the first frame update
    void Start()
    {
        putThisInStart();
    }

    // Update is called once per frame
    void Update()
    {
        putThisInUpdate();
    }

    void FixedUpdate()
    {
        putThisInFixedUpdate();
    }

    public override void action1(bool direction)
    {
        base.action1(direction);
        if (!direction)
        {
            enemiesToDamage = Physics2D.OverlapAreaAll(attackPos1.position, attackPos2.position, enemyLayer);
            var slash = Instantiate(swordSlash, new Vector3(transform.position.x + slashDistance, transform.position.y), transform.rotation);
            slash.GetComponent<SpriteRenderer>().flipY = flipSlash;
            slash.transform.parent = transform;
        }
        else
        {
            enemiesToDamage = Physics2D.OverlapAreaAll(attackPos1.position, attackPos3.position, enemyLayer);
            var slash = Instantiate(swordSlash, new Vector3(transform.position.x - slashDistance, transform.position.y), transform.rotation);
            slash.GetComponent<SpriteRenderer>().flipX = true;
            slash.GetComponent<SpriteRenderer>().flipY = flipSlash;
            slash.transform.parent = transform;
        }

        applyDamage();
    }

    public override void action2(bool direction)
    {
        base.action2(direction);
        enemiesToDamage = Physics2D.OverlapAreaAll(attack2Pos1.position, attack2Pos2.position, enemyLayer);
        var slash = Instantiate(swordSlash, new Vector3(transform.position.x, transform.position.y + slashDistance), Quaternion.Euler(new Vector3(0,0,90)));
        slash.transform.parent = transform;
        slash.GetComponent<SpriteRenderer>().flipY = flipSlash;

        applyDamage();
    }

    void applyDamage()
    {
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyClass>().takeDamage(damage);
        }
        flipSlash = !flipSlash;
    }
}

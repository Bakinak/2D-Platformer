﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyBunScript : InteractEnviClass
{
    public float bunBouncinessY;
    public float bunBouncinessX;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void action(GameObject collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x + bunBouncinessX, bunBouncinessY);
        animator.Play("bouncyBun_Animations_Bounce");
        
    }

}

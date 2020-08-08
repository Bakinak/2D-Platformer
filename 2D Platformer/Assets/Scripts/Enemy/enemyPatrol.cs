using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyClass
{
    public Transform pos1, pos2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void patrol()
    {      
        if (transform.position.x <= pos1.position.x && movementSpeed < 0 || transform.position.x >= pos2.position.x && movementSpeed > 0)
        {
            movementSpeed *= -1;
            flipRender();
        }

        myrigidbody.velocity = new Vector2(movementSpeed, myrigidbody.velocity.y);

    }

    public virtual void flipRender()
    {
        if (movementSpeed < 0) spriterender.flipX = false;
        else if(movementSpeed > 0) spriterender.flipX = true;
    }

    public override void callOnStart()
    {
        base.callOnStart();
        pos1 = transform.GetChild(0);
        pos2 = transform.GetChild(1);
        pos1.parent = null;
        pos2.parent = null;
    }

}

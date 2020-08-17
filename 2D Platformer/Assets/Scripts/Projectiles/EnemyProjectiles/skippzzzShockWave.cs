using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skippzzzShockWave : enemyProjectileClass
{
    public Transform wallCollide;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        checkForWall();
    }
    
    void checkForWall(){
        if(Physics2D.OverlapCircle(wallCollide.position, 0.2f, LayerMask.GetMask("Ground"))){
            Destroy(this.gameObject);
        }
    }
}

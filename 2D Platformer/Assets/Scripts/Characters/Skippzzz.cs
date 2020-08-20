using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skippzzz : Player
{
    public GameObject[] projectiles;
    int nextProjectile;
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
    
    void FixedUpdate(){
        putThisInFixedUpdate();
    }
        
    public override void action1(bool direction){
        base.action1(direction);
        
        if(direction){
            Instantiate(projectiles[nextProjectile], transform.position, Quaternion.Euler(0, 180, 0));
        }
        else{
            Instantiate(projectiles[nextProjectile], transform.position, Quaternion.Euler(0, 0, 0));
        }
        nextProjectile +=1;
        if(nextProjectile > 2) nextProjectile = 0; 
    }
    
    public override void action2(bool direction){
        
    }
    
}

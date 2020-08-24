using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class klatreVægEnemy : EnemyClass
{
    
    public float aggroDistance;
    bool oneTime, charging, shot;
    public float coolDown;
    float timePassed;
    
    public GameObject projectile;
    public Transform projectileSpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        callOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        callOnUpdate();
        
        attack();
    }
    
    void attack(){
        
                
        if(timePassed < coolDown)
        {
            timePassed += Time.deltaTime;
        }
        else
        {
            //Attack!
            if (!charging)
            {
                charging = true;
                animator.Play("Charge");
            }
        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {

            if (!shot)
            {
                shot = true;
                //Spawn projectile, and send it right direction.
                StartCoroutine(waitForAnim(0.5f));
                
                Instantiate(projectile, projectileSpawnPoint.position, transform.rotation);
            }

        }

    }

    IEnumerator waitForAnim(float time)
    {
        yield return new WaitForSeconds(time);
        smallReset();
    }


    public override void respawn()
    {
        base.respawn();
        smallReset();
    }

    void smallReset()
    {
        shot = false;
        charging = false;
        timePassed = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEnviClass : MonoBehaviour
{

    [SerializeField] bool playerTrigger;
    [SerializeField] bool playerProjectileTrigger;
    [SerializeField] bool enemyTrigger;
    [SerializeField] bool enemyProjectileTrigger;

    [SerializeField] bool oneTimeOnly;
    [SerializeField] bool pressAndHold;

    bool alreadyActivated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pressAndHold == false && alreadyActivated == false)
        {
            if(collision.tag == "Player" && playerTrigger || collision.tag == "Enemy" && enemyTrigger || collision.tag == "EnemyProjectile" && enemyProjectileTrigger || collision.tag == "PlayerProjectile" && playerProjectileTrigger)
            {
                action(collision.gameObject);

                if (oneTimeOnly) alreadyActivated = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (pressAndHold && alreadyActivated == false)
        {
            if (collision.tag == "Player" && playerTrigger || collision.tag == "Enemy" && enemyTrigger || collision.tag == "EnemyProjectile" && enemyProjectileTrigger || collision.tag == "PlayerProjectile" && playerProjectileTrigger)
            {
                action(collision.gameObject);

                if (oneTimeOnly) alreadyActivated = true;
            }
        }
    }

    public virtual void action(GameObject collision)
    {

    }

}

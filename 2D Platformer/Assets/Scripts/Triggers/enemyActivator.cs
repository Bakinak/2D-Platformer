using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyActivator : MonoBehaviour
{
    public bool triggerActivated;

    EnemyClass enemyScript;
    public bool activate;
    public Collider2D[] enemiesToActivate;

    EnemyClass enemyClass;

    public Transform point1, point2;



    // Start is called before the first frame update
    void Awake()
    {
        enemiesToActivate = Physics2D.OverlapAreaAll(point1.position, point2.position, LayerMask.GetMask("Enemy"));

    }

    private void Start()
    {
        for (int i = 0; i < enemiesToActivate.Length; i++)
        {
            enemiesToActivate[i].GetComponent<EnemyClass>().enemyActive = false;
            enemiesToActivate[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!triggerActivated)
            {
                activateOrDeactivate();
                triggerActivated = true;
            }
        }
    }


    void activateOrDeactivate()
    {
        if (activate)
        {
            for(int i = 0; i < enemiesToActivate.Length; i++)
            {
                enemiesToActivate[i].GetComponent<EnemyClass>().enemyActive = true;
                enemiesToActivate[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < enemiesToActivate.Length; i++)
            {
                enemiesToActivate[i].GetComponent<EnemyClass>().enemyActive = false;
                enemiesToActivate[i].gameObject.SetActive(false);
            }
        }
    }

}

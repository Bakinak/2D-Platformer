using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    myGameManager manager;
    Animator animator;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<myGameManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void activateOrInactivate(bool activeState)
    {
        active = activeState;
        animator.SetBool("Active", activeState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !active)
        {
            manager.updateCheckPoint(gameObject);
        }
    }

    

}

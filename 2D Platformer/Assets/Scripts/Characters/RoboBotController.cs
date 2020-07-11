using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboBotController : MonoBehaviour
{
    public int animState = 0;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {      
        animator = GetComponent<Animator>();
        animator.SetInteger("animState", animState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeAnimation(int newState)
    {
        animator.SetInteger("animState", newState);
    }

}

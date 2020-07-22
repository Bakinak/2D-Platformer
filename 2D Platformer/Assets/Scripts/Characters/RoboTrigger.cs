using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboTrigger : MonoBehaviour
{

    GameObject roboBot;
    RoboBotController controller;
    public int onEnterChange =-1, onExitChange = -1;
    

    // Start is called before the first frame update
    void Start()
    {
        roboBot = transform.parent.gameObject;
        controller = roboBot.GetComponent<RoboBotController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && onEnterChange != -1) controller.changeAnimation(onEnterChange);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && onExitChange != -1) controller.changeAnimation(onExitChange);
    }
}

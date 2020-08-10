using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrigger : MonoBehaviour
{
    public Transform target;
    public bool bossDoor;

    public bool followX, followY;

    public Transform clampL, clampR, clampT, clampB;

    cameraScript camScript;

    // Start is called before the first frame update
    void Start()
    {
        camScript = GameObject.Find("Main Camera").GetComponent<cameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(target != null) GameObject.Find("Manager").GetComponent<myGameManager>().screenMove(target, bossDoor);
            else
            {
                if (clampL != null) camScript.xAxisClampL = clampL;
                if (clampR != null) camScript.xAxisClampR = clampR;
                if (clampT != null) camScript.yAxisClampT = clampT;
                if (clampB != null) camScript.yAxisClampB = clampB;

                camScript.followX = followX;
                camScript.followY = followY;
            }
        }
    }
}

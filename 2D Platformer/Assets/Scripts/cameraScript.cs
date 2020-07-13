using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

    public Transform target;

    public bool followY = true;
    public bool followX = true;

    public Transform xAxisClampL, xAxisClampR, yAxisClampT, yAxisClampB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followX)
        {
            if(target.transform.position.x > xAxisClampL.position.x && target.transform.position.x < xAxisClampR.position.x)
            transform.position = new Vector3(target.position.x, transform.position.y, -50);
        }

        if (followY)
        {
            if(target.transform.position.y < yAxisClampT.position.y && target.transform.position.y > yAxisClampB.position.y)
            transform.position = new Vector3(transform.position.x, target.transform.position.y, -50);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

    public Transform target;

    public bool followY = true;
    public bool followX = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followX)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, -50);
        }

        if (followY)
        {
            transform.position = new Vector3(transform.position.x, target.transform.position.y, -50);
        }
    }
}

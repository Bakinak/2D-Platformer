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
            if (xAxisClampL != null && xAxisClampR != null) {
                if (target.transform.position.x > xAxisClampL.position.x && target.transform.position.x < xAxisClampR.position.x)
                    transform.position = new Vector3(target.position.x, transform.position.y, -50);
            } else transform.position = new Vector3(target.position.x, transform.position.y, -50);
        }

        if (followY)
        {
            if (yAxisClampT != null && yAxisClampB != null)
            {
                if (target.transform.position.y < yAxisClampT.position.y && target.transform.position.y > yAxisClampB.position.y)
                    transform.position = new Vector3(transform.position.x, target.transform.position.y, -50);
            } else  transform.position = new Vector3(transform.position.x, target.transform.position.y, -50);
        }
    }

    public void moveToPoint(Vector2 pos)
    {
        followX = false;
        followY = false;

        StartCoroutine(moveCamera(pos));
    }

    IEnumerator moveCamera(Vector2 newPos)
    {

        while (transform.position.x != newPos.x && transform.position.y != newPos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPos, 2*Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        yield return null;
    }

}

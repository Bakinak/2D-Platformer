using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

    public Transform target;
    myGameManager manager;

    public bool followY = true;
    public bool followX = true;
    public bool screenMoveSlow;
    bool bossActivate;

    public Transform xAxisClampL, xAxisClampR, yAxisClampT, yAxisClampB;

    Transform saveXClampL, saveXClampR, saveYClampT, saveYClampB, saveTarget;
    Vector3 savePosition;
    bool saveFollowY, saveFollowX;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<myGameManager>();
        savePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (screenMoveSlow)
        {
            slowlyMoveTowards();
        }
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

    public void moveToPoint(Transform pos, bool bossDoor)
    {
        followX = false;
        followY = false;
        bossActivate = bossDoor;
        target = pos;
        screenMoveSlow = true;

    }

    void slowlyMoveTowards()
    {
        if (transform.position.x != target.position.x && transform.position.y != target.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, 10 * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -50);
        }
        else
        {
            screenMoveSlow = false;
            if (bossActivate)
            {
                manager.activateBoss();
                //Activate boss or something here, by calling a method in game manager.
            }

        }
    }

    public void saveCameraSettings()
    {
        saveXClampL = xAxisClampL;
        saveXClampR = xAxisClampR;
        saveYClampB = yAxisClampB;
        saveYClampT = yAxisClampT;

        savePosition = transform.position;
        saveFollowX = followX;
        saveFollowY = followY;
        saveTarget = target;
    }

    public void resetPosition()
    {
        xAxisClampL = saveXClampL;
        xAxisClampR = saveXClampR;
        yAxisClampB = saveYClampB;
        yAxisClampT = saveYClampT;

        transform.position = savePosition;
        followX = saveFollowX;
        followY = saveFollowY;
        target = saveTarget;
    }

}

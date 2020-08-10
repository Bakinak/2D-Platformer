using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float movementSpeed;

    public Transform[] movePoints;
    int currentTarget;
    Vector3 lastPosition;

    Vector3 originalPosition;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        lastPosition = originalPosition;
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        riggedMovement();
    }


    void riggedMovement()
    {
        //Use movetowards.
        transform.position = Vector2.MoveTowards(transform.position, movePoints[currentTarget].position, movementSpeed * Time.deltaTime);
        nextTarget();
    }


    void nextTarget()
    {
        if (transform.position == movePoints[currentTarget].position)
        {
            currentTarget += 1;
            if (currentTarget == movePoints.Length) currentTarget = 0;
        }
    }

    public void resetObject()
    {
        transform.position = originalPosition;
        currentTarget = 0;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFlying : EnemyClass
{

    public bool patrol;
    public Transform[] movePoints;
    int currentTarget;

    public float yMovementSpeed;
    float originalYMovementSpeed;

    public float maxFlyDistanceX;
    public float maxFlyDistanceY;
    float xClampL, xClampR, yClampT, yClampB;

    public float waitTime;
    float timeWaited;
    bool waiting;

    // Start is called before the first frame update
    void Start()
    {
        callOnStart();

        xClampL = transform.position.x - maxFlyDistanceX;
        xClampR = transform.position.x + maxFlyDistanceX;

        yClampB = transform.position.y - maxFlyDistanceY;
        yClampT = transform.position.y + maxFlyDistanceY;

        originalYMovementSpeed = yMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyActive)
        {
            callOnUpdate();

            if (!waiting)
            {
                if (patrol)
                {
                    patrolling();
                }
                else
                {
                    radiusMovement();
                }
            }
            else
            {
                timeWaited += Time.deltaTime;
                if (timeWaited >= waitTime)
                {
                    timeWaited = 0;
                    waiting = false;
                }
            }
        }
    }

    void patrolling()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePoints[currentTarget].position, movementSpeed * Time.deltaTime);

        if (transform.position == movePoints[currentTarget].position)
        {
            currentTarget += 1;
            if (currentTarget == movePoints.Length) currentTarget = 0;
            waiting = true;
        }
    }

    void radiusMovement()
    {
        transform.position += new Vector3(movementSpeed * Time.deltaTime, yMovementSpeed * Time.deltaTime, 0);

        if (maxFlyDistanceX != 0)
        {
            if (transform.position.x <= xClampL || transform.position.x >= xClampR) movementSpeed *= -1;
        }

        if (maxFlyDistanceY != 0)
        {
            if (transform.position.y <= yClampB || transform.position.y >= yClampT) yMovementSpeed *= -1;
        }
    }

    public override void respawn()
    {
        base.respawn();
        timeWaited = 0;
        waiting = false;
        currentTarget = 0;
        yMovementSpeed = originalYMovementSpeed;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mike : Player
{
    // Start is called before the first frame update
    void Start()
    {
        putThisInStart();
    }

    // Update is called once per frame
    void Update()
    {
        putThisInUpdate();
    }

    void FixedUpdate()
    {
        putThisInFixedUpdate();
    }

    public override void action1()
    {
        Debug.Log("1");
    }

    public override void action2()
    {
        Debug.Log("2");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutbackeffect : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 1f * Time.deltaTime, 0);
        if (transform.position.y > 10) transform.position = new Vector3(transform.position.x, -12, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoEdgeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //WHY DOESN'T THIS WORK
        float size = GetComponent<SpriteRenderer>().size.x;

        Vector2[] autoPoints;
        autoPoints = GetComponent<EdgeCollider2D>().points;
        autoPoints[0].x = -size / 2;
        autoPoints[1].x = size / 2;

        GetComponent<EdgeCollider2D>().points = autoPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

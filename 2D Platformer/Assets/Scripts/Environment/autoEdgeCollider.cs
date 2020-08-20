using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoEdgeCollider : MonoBehaviour
{
    public bool scaleY;
    public bool box;
    
    // Start is called before the first frame update
    void Start()
    {
        float size;
        Vector2[] autoPoints;
        autoPoints = GetComponent<EdgeCollider2D>().points;
        
        if(!scaleY)
        {
            size = GetComponent<SpriteRenderer>().size.x;
        } else{
            size = GetComponent<SpriteRenderer>().size.y;
        }

        if(!box){
            autoPoints[0].x = -size / 2;
            autoPoints[1].x = size / 2;
        } else{ //Points on edge collider must start top left, and then loop around for this to work
            autoPoints[0].y = size / 2;
            autoPoints[1].y = size / 2;
            autoPoints[2].y = -size / 2;
            autoPoints[3].y = -size / 2;
            autoPoints[4].y = size / 2;
        }

        GetComponent<EdgeCollider2D>().points = autoPoints;
    }
}

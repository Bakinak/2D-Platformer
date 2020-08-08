using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileClass : soundClass
{
    public float lifeTime;
    float timeLived;

    public float projectileSpeed;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    public virtual void movement()
    {
        transform.position += transform.right * projectileSpeed * Time.deltaTime;
        timeLived += Time.deltaTime;
        if (timeLived > lifeTime) expire();
    }

    public virtual void expire()
    {
        Destroy(gameObject);
    }


}

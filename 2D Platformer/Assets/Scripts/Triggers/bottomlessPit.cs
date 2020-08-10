using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottomlessPit : MonoBehaviour
{

    public Transform respawnPoint;

    public int damage = 2; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Player>().takeDamage(damage);
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.transform.position = respawnPoint.position;
        }
        else if(collision.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
        }
    }

}

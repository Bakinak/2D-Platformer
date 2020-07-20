using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUpClass
{
    public int healingAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void action(GameObject target)
    {
        target.GetComponent<Player>().gainHealth(healingAmount);
        gameObject.SetActive(false);
    }
}

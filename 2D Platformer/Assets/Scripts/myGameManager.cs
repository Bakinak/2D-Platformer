using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myGameManager : MonoBehaviour
{
    public GameObject healthBar;
    public int playerHealth;
    private Animator healthAnim;
    // Start is called before the first frame update
    void Start()
    {
        healthAnim = healthBar.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        changeHealth();
    }

    public void changeHealth()
    {
        healthAnim.SetInteger("health", playerHealth);
    }

}

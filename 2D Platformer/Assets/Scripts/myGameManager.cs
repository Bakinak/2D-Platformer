using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myGameManager : MonoBehaviour
{
    
    //Health
    public int playerHealth;
    int currentHealthDisplayed;
    float healthUpSpeed = 0;

    //UI Stuff
#pragma warning disable 0649
    [SerializeField] GameObject healthBar;
    [SerializeField] Sprite[] healthSprites;
#pragma warning restore
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 16;
        healthBar.GetComponent<Image>().sprite = healthSprites[playerHealth];
        currentHealthDisplayed = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //changeHealth(playerHealth);

        //Health animation
        healthUpSpeed += Time.deltaTime;
        if(healthUpSpeed > 0.05)
        {
            healthUpSpeed = 0;
            if(playerHealth > currentHealthDisplayed)
            {
                currentHealthDisplayed += 1;
                healthBar.GetComponent<Image>().sprite = healthSprites[currentHealthDisplayed];
            } else if(playerHealth < currentHealthDisplayed)
            {
                currentHealthDisplayed -= 1;
                healthBar.GetComponent<Image>().sprite = healthSprites[currentHealthDisplayed];
            }
        }

    }

    public void changeHealth(int healthChange)
    {
        playerHealth += healthChange;
        if (playerHealth > 15) playerHealth = 16;
        else if (playerHealth < 1) playerHealth = 0;
        //healthBar.GetComponent<Image>().sprite = healthSprites[playerHealth];
    }

}

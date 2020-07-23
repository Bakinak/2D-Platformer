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

    GameObject[] levelEnemies;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 16;
        healthBar.GetComponent<Image>().sprite = healthSprites[playerHealth];
        currentHealthDisplayed = playerHealth;
        levelEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        //changeHealth(playerHealth);

        //Health animation
        if (playerHealth != currentHealthDisplayed)
        {
            healthUpSpeed += Time.deltaTime;
            if (healthUpSpeed > 0.05)
            {
                healthUpSpeed = 0;
                if (playerHealth > currentHealthDisplayed)
                {
                    currentHealthDisplayed += 1;
                    healthBar.GetComponent<Image>().sprite = healthSprites[currentHealthDisplayed];
                }
                else if (playerHealth < currentHealthDisplayed)
                {
                    currentHealthDisplayed -= 1;
                    healthBar.GetComponent<Image>().sprite = healthSprites[currentHealthDisplayed];
                }
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


    /*Function that should be called when player dies. What is gonna need to happen:
    Enemies respawn.
    Player respawns at checkpoint.
    Camera moves to player, and keeps the booleans and clamps it had when the player reached said checkpoint, else we might fuck up later.
    Pickups respawn.
    Moving objects reset their position.*/
    void playerDeath()
    {
        playerHealth = 16;
        for(int i = 0; i<levelEnemies.Length; i++)
        {
            levelEnemies[i].GetComponent<EnemyClass>().respawn();
        }
    }

}

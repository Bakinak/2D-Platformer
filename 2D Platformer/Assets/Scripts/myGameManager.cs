using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myGameManager : soundClass
{
    
    //Health
    public int playerHealth;
    int currentHealthDisplayed;
    float healthUpSpeed = 0;

    //UI Stuff
#pragma warning disable 0649
    [SerializeField] GameObject healthBar;
    [SerializeField] Sprite[] healthSprites;
    [SerializeField] GameObject bossHealthBar;
    [SerializeField] Sprite[] bossHealthSprites;
#pragma warning restore
    

    //UI
    public UIcontroller ui;

    //Checkpoints and respawning player
    //Enemies, pickUps, moving platforms.
    public GameObject[] levelEnemies;
    public GameObject[] levelPickUps;
    public GameObject[] movingPlatforms;

    //Checkpoints and player
    GameObject player;
    Player playerScript;
    Vector3 playerSpawnPoint;
    GameObject currentCheckPoint;

    //Camera Information at Check Point Activation.
    public GameObject myCamera;
    cameraScript camScript;
    Vector3 originalCameraPosition;

    public GameObject boss; //Set on start in BossClass.
    public int bossHealthSquare; //How much health the boss must lose before healthbar is updated. Value set in BossClass.
    int bossHealthDisplayed;
    int bossDamageTaken;


    //Sound
#pragma warning disable 0649
    [SerializeField] AudioClip regenBossHealthSound;
    [SerializeField] AudioClip playerHealthUpSound;
    [SerializeField] AudioClip playerDeathSound;
#pragma warning restore

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ui.fadeToBlack(true));
        playerHealth = 16;
        healthBar.GetComponent<Image>().sprite = healthSprites[playerHealth];
        
        currentHealthDisplayed = playerHealth;
        //Finding things we need to respawn on player death.
        levelEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        levelPickUps = GameObject.FindGameObjectsWithTag("pickUp");
        movingPlatforms = GameObject.FindGameObjectsWithTag("movingPlatform");

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerSpawnPoint = player.transform.position;

        myCamera = GameObject.FindGameObjectWithTag("MainCamera");
        camScript = myCamera.GetComponent<cameraScript>();
        originalCameraPosition = myCamera.transform.position;

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
                    if(playerHealthUpSound != null) playSound(playerHealthUpSound);
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
        else if (playerHealth < 1)
        {
            playerHealth = 0;
            //Spawn death animation at player position;
            Instantiate(player.GetComponent<Player>().deathAnim, player.transform.position, player.transform.rotation);
            player.SetActive(false);
            playSound(playerDeathSound);
            StartCoroutine(waitforDeathAnim());
            //playerDeath();
        }
        //healthBar.GetComponent<Image>().sprite = healthSprites[playerHealth];
    }

    IEnumerator waitforDeathAnim()
    {
        float timevar = 0;
        while (timevar < 1.6)
        {
            timevar += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ui.fadeToBlack(true, 2));
    }
    /*Function that should be called when player dies. What is gonna need to happen:
    Enemies respawn.
    Player respawns at checkpoint.
    Camera moves to player, and keeps the booleans and clamps it had when the player reached said checkpoint, else we might fuck up later.
    Pickups respawn.
    Moving objects reset their position.*/
    public void playerDeath()
    {
        if (boss.GetComponent<BossClass>().bossActive)
        {
            bossHealthBar.SetActive(false);
            bossDamageTaken = 0;
        }

        //Respawning Enemies
        for (int i = 0; i<levelEnemies.Length; i++) 
        {
            levelEnemies[i].GetComponent<EnemyClass>().respawn();
        }
        //Pickup Respawn
        for (int i = 0; i < levelPickUps.Length; i++)
        {
            levelPickUps[i].SetActive(true);
        }
        //Resetting moving platforms
        for (int i = 0; i < movingPlatforms.Length; i++)
        {
            //Reset their position. All moving platforms must come from one class that has a function that resets their position.
        }

        //Resetting Player and placing them at checkpoint
        playerHealth = 16;
        player.SetActive(true);
        player.transform.position = playerSpawnPoint;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Player>().died();
        Debug.Log("Player Died");

        //Do something with the camera.
        if (currentCheckPoint == null) myCamera.transform.position = originalCameraPosition;
        camScript.resetPosition();


        StartCoroutine(ui.fadeToBlack(false));
    }

    public void updateCheckPoint(GameObject newCheckPoint)
    {
        if(currentCheckPoint != null)
        {
            currentCheckPoint.GetComponent<CheckPoint>().activateOrInactivate(false);
        }
        currentCheckPoint = newCheckPoint;
        playerSpawnPoint = currentCheckPoint.transform.position;
        camScript.saveCameraSettings();
        currentCheckPoint.GetComponent<CheckPoint>().activateOrInactivate(true);
    }

    //Camera Behaviour. Should call functions in the camera script probably, but may need to go through here first.
    public void screenMove(Transform pos, bool boss)
    {
        playerScript.inControl = false;
        camScript.moveToPoint(pos, boss);
    }

    public void activateBoss()
    {
        StartCoroutine(setupBossEncounter());

    }

    IEnumerator setupBossEncounter()
    {
        bossHealthBar.SetActive(true);

        for (int i = 0; i < bossHealthSprites.Length; i++)
        {
            bossHealthBar.GetComponent<Image>().sprite = bossHealthSprites[i];
            if(regenBossHealthSound!=null) playSound(regenBossHealthSound);
            yield return new WaitForSeconds(0.1f);

        }
        bossHealthDisplayed = 16;

        playerScript.inControl = true;
        boss.GetComponent<BossClass>().bossActive = true;

    }

    public void bossTakeDamage(int damage)
    {
        bossDamageTaken += damage;
        while (bossDamageTaken >= bossHealthSquare)
        {
            bossHealthDisplayed -= 1;
            if(bossHealthDisplayed >= 0) bossHealthBar.GetComponent<Image>().sprite = bossHealthSprites[bossHealthDisplayed];
            bossDamageTaken -= bossHealthSquare;
        }        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myGameManager : soundClass
{
    public GameObject[] playerPrefabs;
    GameObject[] playerInstances;
    public Transform playerStartSpawn;
    int currentCharacter;
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
    public GameObject player;
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


    GameObject[] enemyActivators;


    //Sound
    AudioSource myMusic;
#pragma warning disable 0649
    [SerializeField] AudioClip regenBossHealthSound;
    [SerializeField] AudioClip playerHealthUpSound;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip bossMusic;
#pragma warning restore

    // Start is called before the first frame update
    void Awake()
    {
        //Spawning all available player characters and setting them to inactive except the first one.
        playerInstances = new GameObject[playerPrefabs.Length];
        for(int i = 0; i < playerPrefabs.Length; i++){
            playerInstances[i] = Instantiate(playerPrefabs[i], playerStartSpawn);
            if(i > 0){
                playerInstances[i].GetComponent<Player>().putThisInStart();
                playerInstances[i].SetActive(false);             
            }
        }
        
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

        enemyActivators = GameObject.FindGameObjectsWithTag("ActivatorTrigger");

        //Music
        myMusic = GetComponent<AudioSource>();
        if(levelMusic!=null) myMusic.clip = levelMusic;
        myMusic.Play();
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
    
    //Swapping between characters. Called from the player script.
    public void characterChange(int nextOrPrevious){
        currentCharacter += nextOrPrevious; //Selecting next character.
        if(currentCharacter < 0) currentCharacter = playerInstances.Length - 1;
        else if(currentCharacter > playerInstances.Length - 1) currentCharacter = 0;
        
        if(playerInstances[currentCharacter] != player){
        
            playerInstances[currentCharacter].transform.position = player.transform.position;
            bool flipped = player.GetComponent<SpriteRenderer>().flipX;
            player.SetActive(false);
            
            player = playerInstances[currentCharacter];
            player.SetActive(true);
            playerScript = player.GetComponent<Player>();
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<SpriteRenderer>().flipX = flipped;
            
            //Set camera target to be new player, but only if the target was already a player, so it doesn't mess up any camera moving events.
            if(camScript.target.tag == "Player") camScript.target = player.transform;
            
            playerScript.tagIn();
        }
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
    /*Function that should be called when player dies. Is currently called from UI class I think. What is gonna need to happen:
    Enemies respawn.
    Player respawns at checkpoint.
    Camera moves to player, and keeps the booleans and clamps it had when the player reached said checkpoint, else we might fuck up later.
    Pickups respawn.
    Moving objects reset their position.
    Also remove all projectiles!*/
    public void playerDeath()
    {
        if (boss != null)
        {
            if (boss.GetComponent<BossClass>().bossActive)
            {
                bossHealthBar.SetActive(false);
                bossDamageTaken = 0;
            }
        }

        //Respawning Enemies
        for (int i = 0; i<levelEnemies.Length; i++) 
        {
            if(levelEnemies[i].GetComponent<EnemyClass>().enemyActive)
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
            movingPlatforms[i].GetComponent<movingPlatform>().resetObject();
        }

        //Find all projectiles and delete them, both enemy and player.
        GameObject[] enemyProjectiles = GameObject.FindGameObjectsWithTag("EnemyProjectile");
        for(int i = 0; i < enemyProjectiles.Length; i++)
        {
            Destroy(enemyProjectiles[i]);
        }
        GameObject[] playerProjectiles = GameObject.FindGameObjectsWithTag("PlayerProjectile");
        for (int i = 0; i < playerProjectiles.Length; i++)
        {
            Destroy(playerProjectiles[i]);
        }

        //Resetting Player and placing them at checkpoint
        playerHealth = 16;
        player.SetActive(true);
        player.transform.position = playerSpawnPoint;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Player>().died();
        Debug.Log("Player Died");

        //Do something with the camera.
        camScript.resetPosition();

        //Resetting Enemy Activation Triggers
        for(int i = 0; i < enemyActivators.Length; i++)
        {
            enemyActivators[i].GetComponent<enemyActivator>().triggerActivated = false;
        }

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
        myMusic.clip = bossMusic;
        myMusic.Play();
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

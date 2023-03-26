using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

using Random = UnityEngine.Random;

public class EnemyGeneration : MonoBehaviour
{
    private int maxEnemiesCount = 24; // 24 + 2
    private int currentEnemyCount = 0;

    // Enemy generating
    public GameObject[] zombiePrefab;
    public float enemySpawnInterval = 10f;
    private float heightAboveGround;

    private float maxEnemySpeed = 11f;
    private float currentEnemySpeed;

    private float gameSpeed;
    private Skills playerScript;
    private Movement playerScriptMovenment;
    //private Enemy enemyScript;
    private GameObject player;

    public GameObject Cross;
    public GameObject EndGame;

    // UI
    private float timeScore;
    public Text timerText;

    private int killsCounter;

    // Buffs generating
    public GameObject buff;
    public float buffsSpawnInterval = 35f;

    // Floor texture generating
    public List<Texture> floorTextures = new List<Texture>();
    // Game - default (0)
    // Level 2 - (1)
    // Level 3 - (2)
    // Level 4 - (3)
    public Renderer Floor;

    // Ads
    public InterAd interAd;
    private int triesCount = 0;
    private bool haveAddedTries = false;

    private void Start()
    {
        // Set floor texture
        string levelDetector = "";
        levelDetector = PlayerPrefs.GetString("TextureName");
        switch (levelDetector)
        {
            case "Game":
                Floor.material.mainTexture = floorTextures[0];
                break;
            case "Level 2":
                Floor.material.mainTexture = floorTextures[1];
                break;
            case "Level 3":
                Floor.material.mainTexture = floorTextures[2];
                break;
            case "Level 4":
                Floor.material.mainTexture = floorTextures[3];
                break;
        }

        // Target frames per second
        Application.targetFrameRate = 60;

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Skills>();
        gameSpeed = playerScript.gameSpeed;

        playerScriptMovenment = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        // Spawn a couple (2) of zombies by start
        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(SpawnZombies());
        }

        StartCoroutine(SpawnBuffs());

    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        killsCounter = playerScript.killsCounterInt;

        int playerLevel = PlayerPrefs.GetInt("Level");

        // 10 level = 6.0 interval
        // 30 level = 2.0 interval

        int x = playerLevel;
        enemySpawnInterval = GetDivisors(x);

        float approximatePlayerSpeed = playerLevel / 5;
        if (approximatePlayerSpeed >= 4)
        {
            playerScriptMovenment.movementSpeed = approximatePlayerSpeed;
        }

        if (player != null)
        {
            timeScore += Time.deltaTime / gameSpeed;
            string formatStr = $"Time alive: {Format(timeScore)}";
            timerText.text = formatStr;
        }
        else
        {
            Save();
            haveAddedTries = true;
            
            EndGame.GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(GameOver());

        }
    }

    private void FixedUpdate()
    {
        float playerSpeed = playerScriptMovenment.movementSpeed;
        currentEnemySpeed = playerSpeed - 0.35f;

        int level = PlayerPrefs.GetInt("Level");
        playerScript.meteorLevel = level;
        playerScript.tornadoLevel = level;
        playerScript.waveLevel = level;
        playerScript.fireLevel = level;
        playerScript.ultimateLevel = level;

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            EnemyMovement enemyComponent = enemy.GetComponent<EnemyMovement>();
            if (enemyComponent != null)
            {
                if (enemyComponent.navMeshAgent.velocity.magnitude <= maxEnemySpeed)
                {
                    enemyComponent.navMeshAgent.speed = currentEnemySpeed;
                }
            }
        }

    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator SpawnZombies()
    {
        // Generate loop
        while (currentEnemyCount < maxEnemiesCount)
        {
            // Generate random position in sphere 
            float radius = 35f;
            Vector3 randomPosition = Random.insideUnitSphere * radius;

            float randomX = randomPosition.x;
            float randomZ = randomPosition.z;

            float rotationY = Random.Range(-180, 180);

            Quaternion randomRotation = new Quaternion(0, rotationY, 0, 0);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomX, 64f, randomZ), Vector3.down, out hit))
            {
                heightAboveGround = hit.distance;
            }

            Vector3 spawnPos = new Vector3(randomX, 64 - heightAboveGround, randomZ);

            GameObject cross = Instantiate(Cross, spawnPos, randomRotation, transform);
            yield return new WaitForSeconds(enemySpawnInterval / 2);
            Destroy(cross);

            // Generate random zombie by chance
            int rand = Random.Range(0, 100);
            if (rand >= 0 && rand < 40) rand = 0;
            else if (rand >= 40 && rand < 80) rand = 1;
            else if (rand >= 80 && rand < 100) rand = 2;

            Instantiate(zombiePrefab[rand], spawnPos, randomRotation, transform);
            yield return new WaitForSeconds(enemySpawnInterval / 2);

            currentEnemyCount++;

        }
    }

    private IEnumerator SpawnBuffs()
    {
        // Generate loop
        while (true)
        {
            // Generate random position in sphere 
            float radius = 35f;
            Vector3 randomPosition = Random.insideUnitSphere * radius;

            float randomX = randomPosition.x;
            float randomZ = randomPosition.z;

            float rotationY = Random.Range(-180, 180);

            Quaternion randomRotation = new Quaternion(0, rotationY, 0, 0);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomX, 64f, randomZ), Vector3.down, out hit))
            {
                heightAboveGround = hit.distance;
            }

            Vector3 spawnPos = new Vector3(randomX, 64 - heightAboveGround, randomZ);

            GameObject cross = Instantiate(Cross, spawnPos, randomRotation, transform);
            cross.transform.localScale /= 1.5f;
            yield return new WaitForSeconds(buffsSpawnInterval / 2);
            Destroy(cross);

            Instantiate(buff, spawnPos, randomRotation, transform);
            yield return new WaitForSeconds(buffsSpawnInterval / 2);
        }
    }

    private static string Format(float seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);

        if (ts.Hours != 0) return $"{ts.Hours} h, {ts.Minutes} m, {ts.Seconds} s";
        else if (ts.Minutes != 0) return $"{ts.Minutes} m, {ts.Seconds} s";
        else if (ts.Seconds != 0) return $"{ts.Seconds} s";

        return "0 s";
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    // Best score save & set tries count
    public void Save()
    {
        if (haveAddedTries == false)
        {
            // Get and set tries count
            triesCount = PlayerPrefs.GetInt("triesCount");
            triesCount++;
            PlayerPrefs.SetInt("triesCount", triesCount);

            // Show ad if the player died every 3 times
            if (triesCount % 3 == 0)
            {
                interAd.ShowAd();
            }
        }

        float previousBestScore = PlayerPrefs.GetFloat("BestScore");
        if (previousBestScore < timeScore)
        {
            PlayerPrefs.SetFloat("BestScore", timeScore);
            PlayerPrefs.SetInt("BestScoreKills", killsCounter);
        }
    }

    public static float GetDivisors(int number)
    {
        float n = 0;

        // 10 - 30
        n = number - 40;

        // -30 - -10
        n *= -1;

        // 30 - 10;
        n /= 5;
        /*
        // 60 - 20
        n *= 2;
        */
        return n;
    }

}

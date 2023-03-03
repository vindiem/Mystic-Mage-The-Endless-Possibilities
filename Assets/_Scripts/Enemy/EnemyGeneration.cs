using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 10f;
    private float heightAboveGround;

    private float maxMutantSpeed = 11f;
    private float currentMutantSpeed;

    private float gameSpeed;
    private Skills playerScript;
    private Movement playerScriptMovenment;
    //private Enemy enemyScript;
    private GameObject player;

    private float timeScore;
    public Text timerText;

    private int killsCounter;

    public GameObject Cross;
    public GameObject EndGame;

    private void Start()
    {
        // Frames per second
        Application.targetFrameRate = 60;

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Skills>();
        gameSpeed = playerScript.gameSpeed;

        playerScriptMovenment = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        StartCoroutine(SpawnZombies());

    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        killsCounter = playerScript.killsCounterInt;

        int playerLevel = PlayerPrefs.GetInt("Level");
        float x = playerLevel;
        float y = 10 - 0.35f * (x - 10);

        spawnInterval = y;
        playerScriptMovenment.movementSpeed = playerLevel / 5;

        if (player != null)
        {
            timeScore += Time.deltaTime / gameSpeed;
            string formatStr = $"Time alive: {Format(timeScore)}";
            timerText.text = formatStr;
        }
        else
        {
            float previousBestScore = PlayerPrefs.GetFloat("BestScore");
            if (previousBestScore < timeScore)
            {
                PlayerPrefs.SetFloat("BestScore", timeScore);
                PlayerPrefs.SetInt("BestScoreKills", killsCounter);
            }
            
            EndGame.GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(GameOver());

        }
    }

    private void FixedUpdate()
    {
        float playerSpeed = playerScriptMovenment.movementSpeed;
        currentMutantSpeed = playerSpeed - 0.15f;

        int level = PlayerPrefs.GetInt("Level");
        playerScript.meteorLevel = level;
        playerScript.tornadoLevel = level;
        playerScript.waveLevel = level;
        playerScript.fireLevel = level;
        playerScript.ultimateLevel = level;

        /*if (playerSpeed > 5 && playerSpeed < 6)
        {
            playerScript.meteorLevel = 15;
            playerScript.tornadoLevel = 15;
            playerScript.waveLevel = 15;
            playerScript.fireLevel = 15;
            playerScript.ultimateLevel = 15;
        }
        else if (playerSpeed > 6 && playerSpeed < 7)
        {
            playerScript.meteorLevel = 20;
            playerScript.tornadoLevel = 20;
            playerScript.waveLevel = 20;
            playerScript.fireLevel = 20;
            playerScript.ultimateLevel = 20;
        }
        else if (playerSpeed > 7 && playerSpeed < 8)
        {
            playerScript.meteorLevel = 25;
            playerScript.tornadoLevel = 25;
            playerScript.waveLevel = 25;
            playerScript.fireLevel = 25;
            playerScript.ultimateLevel = 25;
        }
        else if (playerSpeed > 8 && playerSpeed < 9)
        {
            playerScript.meteorLevel = 30;
            playerScript.tornadoLevel = 30;
            playerScript.waveLevel = 30;
            playerScript.fireLevel = 30;
            playerScript.ultimateLevel = 30;
        }*/

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObject in enemyObjects)
        {
            Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                if (enemyComponent.navMeshAgent.velocity.magnitude <= maxMutantSpeed)
                {
                    enemyComponent.navMeshAgent.speed = currentMutantSpeed;
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
            yield return new WaitForSeconds(spawnInterval / 2);
            Destroy(cross);
            Instantiate(zombiePrefab, spawnPos, randomRotation, transform);
            yield return new WaitForSeconds(spawnInterval / 2);
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
        float previousBestScore = PlayerPrefs.GetFloat("BestScore");
        if (previousBestScore < timeScore)
        {
            PlayerPrefs.SetFloat("BestScore", timeScore);
            PlayerPrefs.SetInt("BestScoreKills", killsCounter);
        }
    }

}

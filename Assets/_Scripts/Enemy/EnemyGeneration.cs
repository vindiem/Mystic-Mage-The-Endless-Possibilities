using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject[] zombiePrefab;
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

    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        killsCounter = playerScript.killsCounterInt;

        int playerLevel = PlayerPrefs.GetInt("Level");

        // 10 level = 6.0 interval
        // 30 level = 2.0 interval

        int x = playerLevel;
        spawnInterval = GetDivisors(x);

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
            int rand = Random.Range(0, zombiePrefab.Length);
            Instantiate(zombiePrefab[rand], spawnPos, randomRotation, transform);
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
        Save();
    }

    // Best score save
    public void Save()
    {
        float previousBestScore = PlayerPrefs.GetFloat("BestScore");
        if (previousBestScore < timeScore)
        {
            PlayerPrefs.SetFloat("BestScore", timeScore);
            PlayerPrefs.SetInt("BestScoreKills", killsCounter);
        }
    }

    public static float GetDivisors(int number)
    {
        float n;
        // 10 - 30
        n = number - 40;
        // -30 - -10
        n *= -1;
        // 30 - 10;
        n /= 5;

        return n;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 10f;
    private float minSpawnInterval = 1.5f;
    private float heightAboveGround;

    private float maxPlayerSpeed = 12f;
    private float maxMutantSpeed = 11f;

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
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Skills>();
        gameSpeed = playerScript.gameSpeed;

        playerScriptMovenment = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();

        StartCoroutine(SpawnZombies());

    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        killsCounter = playerScript.killsCounterInt;

        if (player != null)
        {
            timeScore += Time.deltaTime / gameSpeed;
            timerText.text = timeScore.ToString();
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
        if (spawnInterval >= minSpawnInterval)
        {
            spawnInterval -= 0.0001f;
        }
        if (playerScriptMovenment.movementSpeed <= maxPlayerSpeed)
        {
            playerScriptMovenment.movementSpeed += 0.0001f;
        }

        float speed = playerScriptMovenment.movementSpeed;

        if (speed > 5 && speed < 6)
        {
            playerScript.meteorLevel = 15;
            playerScript.tornadoLevel = 15;
            playerScript.waveLevel = 15;
            playerScript.fireLevel = 15;
            playerScript.ultimateLevel = 15;
        }
        else if (speed > 6 && speed < 7)
        {
            playerScript.meteorLevel = 20;
            playerScript.tornadoLevel = 20;
            playerScript.waveLevel = 20;
            playerScript.fireLevel = 20;
            playerScript.ultimateLevel = 20;
        }
        else if (speed > 7 && speed < 8)
        {
            playerScript.meteorLevel = 25;
            playerScript.tornadoLevel = 25;
            playerScript.waveLevel = 25;
            playerScript.fireLevel = 25;
            playerScript.ultimateLevel = 25;
        }
        else if (speed > 8 && speed < 9)
        {
            playerScript.meteorLevel = 30;
            playerScript.tornadoLevel = 30;
            playerScript.waveLevel = 30;
            playerScript.fireLevel = 30;
            playerScript.ultimateLevel = 30;
        }

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObject in enemyObjects)
        {
            Enemy enemyComponent = enemyObject.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                if (enemyComponent.navMeshAgent.velocity.magnitude <= maxMutantSpeed)
                {
                    enemyComponent.navMeshAgent.speed += 0.0001f;
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

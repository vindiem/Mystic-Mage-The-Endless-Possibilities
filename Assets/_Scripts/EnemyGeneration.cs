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

    private float gameSpeed;
    private Skills playerScript;
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
        killsCounter = playerScript.killsCounterInt;

        StartCoroutine(SpawnZombies());

    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            timeScore += Time.deltaTime * gameSpeed;
            timerText.text = timeScore.ToString("000");
        }
        else
        {
            float previousBestScore = PlayerPrefs.GetFloat("BestScore");
            if (previousBestScore < timeScore)
            {
                PlayerPrefs.SetFloat("BestScore", timeScore);

                float previousBestKillsScore = PlayerPrefs.GetInt("BestScoreKills");
                if (previousBestKillsScore < killsCounter)
                {
                    PlayerPrefs.SetInt("BestScoreKills", killsCounter);
                }
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
            float radius = 40f;
            Vector3 randomPosition = Random.insideUnitSphere * radius;

            float randomX = randomPosition.x;
            float randomZ = randomPosition.z;

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomX, 64f, randomZ), Vector3.down, out hit))
            {
                heightAboveGround = hit.distance;
            }

            Vector3 spawnPos = new Vector3(randomX, 64 - heightAboveGround, randomZ);

            GameObject cross = Instantiate(Cross, spawnPos, Quaternion.identity, transform);
            yield return new WaitForSeconds(spawnInterval / 2);
            Destroy(cross);
            Instantiate(zombiePrefab, spawnPos, Quaternion.identity, transform);
            yield return new WaitForSeconds(spawnInterval / 2);
        }
    }

    private void OnApplicationQuit()
    {
        float previousBestScore = PlayerPrefs.GetFloat("BestScore");
        if (previousBestScore < timeScore)
        {
            PlayerPrefs.SetFloat("BestScore", timeScore);

            float previousBestKillsScore = PlayerPrefs.GetInt("BestScoreKills");
            if (previousBestKillsScore < killsCounter)
            {
                PlayerPrefs.SetInt("BestScoreKills", killsCounter);
            }
        }
    }

}

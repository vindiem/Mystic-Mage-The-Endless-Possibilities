using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 2f;

    public float time;
    public Text timerText;

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    private void Update()
    {
        time += Time.deltaTime;
        timerText.text = time.ToString("000");
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            float randomX = Random.Range(-20f, 20f);
            float randomZ = Random.Range(-20f, 20f);
            Vector3 spawnPos = new Vector3(randomX, 0.55f, randomZ);

            Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

}

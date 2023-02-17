using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 10f;
    private float heightAboveGround;

    private float time;
    public Text timerText;

    public GameObject Cross;

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
        // Generate loop
        while (true)
        {
            float randomX = Random.Range(-50 / 2, 50 / 2);
            float randomZ = Random.Range(-50 / 2, 50 / 2);

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

}

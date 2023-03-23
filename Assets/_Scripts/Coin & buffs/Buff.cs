using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private static int health = 30;
    private static int posion = 20;
    private int randomBuffType = 0;

    private void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        randomBuffType = Random.Range(0, 2);

        switch (randomBuffType)
        {
            case 0:
                particleSystem.startColor = Color.green;
                break;
            case 1:
                particleSystem.startColor = Color.red;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (randomBuffType == 0)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Skills>().ChangeHealth(health);
            }
            else if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyMovement>().ChangeHealth(health * 2);
            }
        }
        else if (randomBuffType == 1)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Skills>().TakeDamage(posion);
            }
            else if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyMovement>().TakeDamage(posion * 2);
            }
        }
        Destroy(gameObject);

    }

}

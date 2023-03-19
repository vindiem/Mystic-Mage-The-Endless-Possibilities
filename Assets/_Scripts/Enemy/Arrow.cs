using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 10;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.LookAt(new Vector3(player.position.x, 0, player.position.z));

        // Randomize damage using mutants on scene
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            EnemyAttackSystem enemyComponent = enemy.GetComponent<EnemyAttackSystem>();
            if (enemyComponent != null)
            {
                damage = enemyComponent.Damage; 
                break;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") == true)
        {
            collision.gameObject.GetComponent<Skills>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

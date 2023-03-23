using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 10;
    private Transform player;

    // Explotion variables
    public float radius = 5.0f;
    public float power = 10.0f;
    public float upwardsModifier = 3.0f;
    public GameObject explotionVFX;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.LookAt(player.position);

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
            collision.gameObject.GetComponent<Skills>().TakeDamage(damage / 2);
        }
        else if (collision.collider.CompareTag("Enemy") == true)
        {
            collision.gameObject.GetComponent<EnemyMovement>().TakeDamage(damage * 2);
        }

        // Explotion by chance
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            Explode();
        }
        Destroy(gameObject);
    }

    private void Explode()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);
        foreach (Collider hit in colliders)
        {   
            if (hit.CompareTag("Player") == true)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPosition, radius, upwardsModifier, ForceMode.Impulse);
                }
                hit.GetComponent<Skills>().TakeDamage(damage / 4);
            }
            else if (hit.CompareTag("Enemy") == true)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPosition, radius, upwardsModifier, ForceMode.Impulse);
                }
                hit.GetComponent<EnemyMovement>().TakeDamage(damage);
            }

        }

        GameObject effect = Instantiate(explotionVFX, transform.position, Quaternion.identity);
        Destroy(effect, 4f);
        
    }

}

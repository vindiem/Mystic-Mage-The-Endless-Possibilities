using System.Collections;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    private float fireRate = 5f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    private Transform player;

    private float nextFireTime = 0f;
    public int seeRange = 0;
    public int runningRange = 0;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    private void Update()
    {
        // Run to player 
        // if 10 (see range) < distance to player > 15 (running range) 
        if (enemyMovement.distance > runningRange && enemyMovement.distance < seeRange &&
            enemyMovement.navMeshAgent.enabled == true)
        {
            enemyMovement.navMeshAgent.enabled = true;
            enemyMovement.navMeshAgent.SetDestination(player.position);
        }
        else if (enemyMovement.navMeshAgent.enabled == true)
        {
            enemyMovement.navMeshAgent.enabled = false;
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion offsetRotation = Quaternion.Euler(0f, 90f, 0f);
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion finalRotation = lookRotation * offsetRotation;
            transform.rotation = finalRotation;
        }

        // Shooting logic
        if (Time.time >= nextFireTime)
        {
            enemyMovement.animator.SetBool("isAttacking", true);
            nextFireTime = Time.time + fireRate;
        }

    }

    private void Shoot()
    {
        Vector3 playerFrontIndicator = player.position;
        if (player.GetComponent<Rigidbody>().velocity != new Vector3(0, 0, 0))
        {
            playerFrontIndicator += player.forward * 1.5f;
        }

        Vector3 futurePlayerPosition = playerFrontIndicator + player.GetComponent<Rigidbody>().velocity;
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();

        Vector3 directionToPlayer = futurePlayerPosition - firePoint.position;
        arrowRb.velocity = directionToPlayer.normalized * 25;
    }

}
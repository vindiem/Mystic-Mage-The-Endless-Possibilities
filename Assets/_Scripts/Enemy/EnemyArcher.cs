using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyArcher : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    private float fireRate = 5f;
    public GameObject arrowPrefab;
    public Transform firePoint;
    private Transform player;

    private float nextFireTime = 0f;
    public int seeRange = 0;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    private void Update()
    {
        if (player == null) return;

        // is archer has shooted -> archer going to player
        if (enemyMovement.distance <= seeRange)
        {
            enemyMovement.navMeshAgent.SetDestination(player.position);
        }

        #region Rotation 

        if (enemyMovement.speed > 2f)
        {
            ArcherRotation(false);
        }
        else if (enemyMovement.speed < 1f)
        {
            ArcherRotation(true);
        }

        #endregion

        #region Shooting logic

        if (Time.time >= nextFireTime && 
            enemyMovement.distance <= enemyMovement.navMeshAgent.stoppingDistance + 1)
        {
            enemyMovement.animator.SetBool("isAttacking", true);
            nextFireTime = Time.time + fireRate;
        }
        else
        {
            enemyMovement.animator.SetBool("isAttacking", false);
        }

        #endregion

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

    private void ArcherRotation(bool isAttacking)
    {
        // Cool rotation (using offset) (using while archer is aiming)
        if (isAttacking == true)
        {
            float offset = 90f;

            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion offsetRotation = Quaternion.Euler(0f, offset, 0f);
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            Quaternion finalRotation = lookRotation * offsetRotation;
            transform.rotation = finalRotation;
        }

        // Simple rotation (using while arhcer is moving)
        else if (isAttacking == false)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

}
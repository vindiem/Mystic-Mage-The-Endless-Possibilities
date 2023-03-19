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
    public int runningRange = 0;
    public int maxShootDistance = 0;

    private bool hasShooted = true;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    private void Update()
    {
        if (player == null) return;

        // is archer has shooted -> archer will going to player
        if (hasShooted == true)
        {
            enemyMovement.navMeshAgent.SetDestination(player.position);
        }

        #region Rotation 

        if (enemyMovement.navMeshAgent.velocity.magnitude > 2)
        {
            ArcherRotation(false);
        }
        else if (enemyMovement.navMeshAgent.velocity.magnitude < 1)
        {
            ArcherRotation(true);
        }

        #endregion

        #region Shooting logic

        if (Time.time >= nextFireTime && enemyMovement.distance <= maxShootDistance)
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

        StartCoroutine(SetTargetActive());

    }

    private void ArcherRotation(bool isAttacking)
    {
        // Cool rotation (using offset) (using while archer is aiming)
        if (isAttacking == true)
        {
            hasShooted = false;
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
            transform.LookAt(player.position);
        }
    }

    private IEnumerator SetTargetActive()
    {
        yield return new WaitForSeconds(0.75f);
        hasShooted = true;
    }

}
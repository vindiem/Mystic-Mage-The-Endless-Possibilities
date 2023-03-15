using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackSystem : MonoBehaviour
{
    private EnemyUI enemyUI;
    private EnemyMovement enemyMovement;

    private int EditedDamage = 10;
    public int Damage = 10;

    private float maxAttackRange = 4.25f;
    private float seeRange = 15f;

    private float attackRate = 4f;
    private float nextAttackTime;

    public float markerPlacementRadius = 5f;
    [HideInInspector] public GameObject marker;
    public GameObject markerPrefab;
    public GameObject damageColliderPrefab;
    private GameObject damageCollider;

    private Transform player;

    private void Start()
    {
        enemyUI = GetComponent<EnemyUI>();
        enemyMovement = GetComponent<EnemyMovement>();

        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Randomize Damage
        Damage = Random.Range(EditedDamage - 4, EditedDamage + 3);

        // Attack
        if (enemyMovement.distance <= seeRange && enemyMovement.navMeshAgent.enabled == true)
        {
            if (marker == null)
            {
                enemyMovement.navMeshAgent.SetDestination(player.position);
                Destroy(damageCollider);
            }

            if (Time.time >= nextAttackTime && enemyMovement.distance < maxAttackRange)
            {
                // Running sound
                enemyMovement.runningSource.mute = true;

                // Instantiate marker
                PlaceMarker();

                #region Difference attack animations

                enemyMovement.navMeshAgent.isStopped = true;

                int rand = Random.Range(1, 5);
                enemyMovement.animator.SetInteger("AttackInt", rand);
                enemyMovement.animator.SetBool("isAttacking", true);
                StartCoroutine(AttackOff());

                #endregion

                // Attack sound play
                enemyMovement.m_audioSource.PlayOneShot(enemyMovement.attackSound);

                // Set next time attack, so that the zombie doesn't hit the player every time
                nextAttackTime = Time.time + attackRate;

            }
            else if (enemyMovement.distance > maxAttackRange)
            {
                enemyMovement.navMeshAgent.isStopped = false;

                // Running sound
                enemyMovement.runningSource.mute = false;
            }

        }

    }

    // Instantiate Marker in true position
    private void PlaceMarker()
    {
        // Instantiate marker on random position near by player radius (markerPlacementRadius)
        Vector3 randomDirection = Random.insideUnitSphere * markerPlacementRadius;
        Vector3 markerPosition = player.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(markerPosition, out hit, markerPlacementRadius, NavMesh.AllAreas))
        {
            marker = Instantiate(markerPrefab, hit.position, Quaternion.identity);

            // Destroy marker if zombie was killed before he attacked player
            Destroy(marker, 2f);
        }

        // Set target position to marker
        if (marker != null)
        {
            enemyMovement.navMeshAgent.SetDestination(marker.transform.position);
        }

    }

    // Using only in animations
    private void AttackMarker()
    {
        if (marker != null)
        {
            damageCollider = Instantiate(damageColliderPrefab, marker.transform.position, Quaternion.identity);
            Destroy(marker, 0.1f);
            Destroy(damageCollider, 0.25f);
        }
    }

    // Coroutines
    private IEnumerator AttackOff()
    {
        yield return new WaitForSeconds(0.25f);
        enemyMovement.animator.SetBool("isAttacking", false);
    }

}

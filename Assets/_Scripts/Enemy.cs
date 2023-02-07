using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    private int damage = 10;

    private float attackRange = 3f;
    private float seeRange = 10f;

    private float attackRate = 3.5f;
    private float nextAttackTime;

    private float distance;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;

    public enum Element
    {
        Earth,
        Air,
        Fire,
        Water
    };

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= seeRange)
        {
            navMeshAgent.SetDestination(player.position);

            if (Time.time >= nextAttackTime && distance <= attackRange)
            {
                animator.SetBool("isAttacking", true);
                navMeshAgent.isStopped = true;

                // Difference attack animations
                int rand = Random.Range(1, 5);
                animator.SetInteger("AttackInt", rand);

                nextAttackTime = Time.time + attackRate;
                // Take damage
            }
            else if (distance >= attackRange)
            {
                navMeshAgent.isStopped = false;
                animator.SetBool("isAttacking", false);
            }
        }


        // Death
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            navMeshAgent.isStopped = true;
            Destroy(gameObject, 4f);
        }

    }

    public void TakeDamage()
    {
        player.GetComponent<OutVoker>().TakeDamage(damage);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float attackRange = 3f;
    public float seeRange = 5f;
    public float attackRate = 1f;

    private float nextAttackTime;
    [SerializeField] private float distance;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
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
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Attack");

                nextAttackTime = Time.time + attackRate;
                player.GetComponent<OutVoker>().TakeDamage(damage);
            }
            else if (distance >= attackRange)
            {
                navMeshAgent.isStopped = false;
            }
        }

    }
}
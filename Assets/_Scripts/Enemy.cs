using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    private int damage = 10;

    private float minAttackRange = 3.75f;
    private float maxAttackRange = 4.25f;
    private float seeRange = 10f;

    private float attackRate = 3.5f;
    private float nextAttackTime;

    private float distance;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;
    private OutVoker playerScript;

    public enum Element
    {
        Earth,
        Air,
        Fire,
        Water
    };

    private Element element;
    private Material material;

    public Image healthBackground;
    public Image healthImage;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        playerScript = player.GetComponent<OutVoker>();

        material = GetComponentInChildren<Renderer>().material;
        int randomElement = Random.Range(0, 4);
        switch (randomElement)
        {
            case 0:
                element = Element.Air;
                material.SetColor("_EmissionColor", new Color(32, 32, 32) * 0.01f);
                break;
            case 1:
                element = Element.Earth;
                material.SetColor("_EmissionColor", new Color(32, 27, 11) * 0.01f);
                break;
            case 2:
                element = Element.Fire;
                material.SetColor("_EmissionColor", new Color(32, 13, 11) * 0.01f);
                break;
            case 3:
                element = Element.Water;
                material.SetColor("_EmissionColor", new Color(11, 30, 32) * 0.01f);
                break;
        }

    }

    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        distance = Vector3.Distance(transform.position, player.position);

        if (distance <= seeRange)
        {
            navMeshAgent.SetDestination(player.position);

            if (Time.time >= nextAttackTime && distance >= minAttackRange && distance <= maxAttackRange)
            {
                animator.SetBool("isAttacking", true);
                navMeshAgent.isStopped = true;

                // Difference attack animations
                int rand = Random.Range(1, 5);
                animator.SetInteger("AttackInt", rand);

                nextAttackTime = Time.time + attackRate;
                // Take damage to hero
            }
            else if (distance >= maxAttackRange)
            {
                navMeshAgent.isStopped = false;
                animator.SetBool("isAttacking", false);
            }
            else if (distance <= minAttackRange)
            {
                navMeshAgent.isStopped = true;
                animator.SetBool("isAttacking", true);
            }
        }

        LookAtPlayer();

        // Death
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            navMeshAgent.isStopped = true;
            Destroy(gameObject, 4f);
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        healthBackground.transform.LookAt(cameraPosition);
        healthImage.transform.LookAt(cameraPosition);
        healthImage.fillAmount = health / 100;
    }

    public void TakeDamageToHero()
    {
        player.GetComponent<OutVoker>().TakeDamage(damage);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ; element - zombie >> elements that match

        if (other.CompareTag("Meteor") == true)
        {
            if (element == Element.Water)
            {
                TakeDamage(playerScript.meteorLevel * 2);
            }
            else if (element != Element.Water)
            {
                TakeDamage(playerScript.meteorLevel / 3 * 2);
            }
            
        }

        else if (other.CompareTag("Tornado") == true)
        {
            if (element == Element.Fire)
            {
                TakeDamage(playerScript.tornadoLevel * 2);
            }
            else if (element != Element.Fire)
            {
                TakeDamage(playerScript.tornadoLevel / 3 * 2);
            }
        }

        else if (other.CompareTag("Wave") == true)
        {
            if (element == Element.Earth)
            {
                TakeDamage(playerScript.waveLevel * 2);
            }
            else if (element != Element.Earth)
            {
                TakeDamage(playerScript.waveLevel / 3 * 2);
            }
        }

        else if (other.CompareTag("Fire") == true)
        {
            if (element == Element.Air)
            {
                TakeDamage(playerScript.fireLevel * 2);
            }
            else if (element != Element.Air)
            {
                TakeDamage(playerScript.fireLevel / 3 * 2);
            }
        }

        // Ultimate damage
        else if (other.CompareTag("Ultimate") == true)
        {
            TakeDamage(playerScript.ultimateLevel * 2);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Meteor") == true)
        {
            //TakeDamage(playerScript.meteorLevel / 10);
        }
    }

    private IEnumerator TornadoFeature()
    {
        yield return new WaitForSeconds(1f);
    }

}
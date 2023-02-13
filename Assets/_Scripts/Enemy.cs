using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    public int damage = 10;

    private float minAttackRange = 3.75f;
    private float maxAttackRange = 4.25f;
    private float seeRange = 14.5f;

    private float attackRate = 2.75f;
    private float nextAttackTime;

    private float distance;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;
    private OutVoker playerScript;
    private Rigidbody rb;

    public Transform backLandmark;
    public Text elementText;

    public float markerPlacementRadius = 5f;
    private GameObject marker;
    public GameObject markerPrefab;
    private bool isAttacking = false;
    public GameObject damageColliderPrefab;
    private GameObject damageCollider;

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
        rb = GetComponent<Rigidbody>();
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
                material.SetColor("_EmissionColor", new Color(32, 32, 32) * 0.014f);
                elementText.text = "Air".ToString();
                elementText.color = Color.white;
                break;
            case 1:
                element = Element.Earth;
                material.SetColor("_EmissionColor", new Color(32, 27, 11) * 0.014f);
                elementText.text = "Earth".ToString();
                elementText.color = Color.gray;
                break;
            case 2:
                element = Element.Fire;
                material.SetColor("_EmissionColor", new Color(32, 13, 11) * 0.014f);
                elementText.text = "Fire".ToString();
                elementText.color = Color.red;
                break;
            case 3:
                element = Element.Water;
                material.SetColor("_EmissionColor", new Color(11, 30, 32) * 0.014f);
                elementText.text = "Water".ToString();
                elementText.color = Color.cyan;
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
        if (navMeshAgent.enabled == false)
        {
            return;
        }

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        #region Set float distance

        // if marker is instantiate -> target - marker
        // if marker isn't instantiate -> target - player
        if (marker == null)
        {
            distance = Vector3.Distance(transform.position, player.position);
        }
        else if (marker != null)
        {
            distance = Vector3.Distance(transform.position, marker.transform.position);

            LookAtTarget(marker);
        }

        #endregion

        if (distance <= seeRange)
        {
            if (marker == null)
            {
                navMeshAgent.SetDestination(player.position);

                Destroy(damageCollider, 0.15f);
            }

            if (Time.time >= nextAttackTime /*&& distance >= minAttackRange*/ && distance <= maxAttackRange)
            {
                PlaceMarker();

                // Set target position to marker
                navMeshAgent.SetDestination(marker.transform.position);

                #region Difference attack animations

                isAttacking = true;

                animator.SetBool("isAttacking", isAttacking);
                navMeshAgent.isStopped = true;

                int rand = Random.Range(1, 5);
                animator.SetInteger("AttackInt", rand);

                #endregion

                nextAttackTime = Time.time + attackRate;
            }

            else if (distance <= minAttackRange)
            {
                navMeshAgent.isStopped = true;
                isAttacking = true;
            }

            else if (distance >= maxAttackRange)
            {
                navMeshAgent.isStopped = false;
                isAttacking = false;
            }

        }

        // Death
        if (health <= 0 || transform.position.y <= -10f)
        {
            transform.GetComponent<Collider>().enabled = false;
            animator.SetTrigger("Death");
            navMeshAgent.isStopped = true;
            Destroy(gameObject, 4f);
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        healthBackground.transform.LookAt(cameraPosition);
        healthImage.transform.LookAt(cameraPosition);
        elementText.transform.LookAt(cameraPosition);
        healthImage.fillAmount = health / 100;

        animator.SetBool("isAttacking", isAttacking);
    }

    public void TakeDamageToHero()
    {
        player.GetComponent<OutVoker>().TakeDamage(damage);
    }

    private void LookAtTarget(GameObject target)
    {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }

    private void TakeDamage(int damage)
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
                TakeDamage(playerScript.tornadoLevel);
            }
            else if (element != Element.Fire)
            {
                TakeDamage(playerScript.tornadoLevel / 3 * 2);
            }

            GameObject t = Instantiate(playerScript.visualTornado, transform.position, 
                Quaternion.LookRotation(Vector3.up));

            Destroy(t, 1.5f);

            StartCoroutine(Raise());
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

            Vector3 backDirection = (transform.position - backLandmark.position).normalized;
            rb.AddForce(-backDirection * playerScript.waveLevel * 16);
            StartCoroutine(Freez(playerScript.waveLevel / 12));
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
            StartCoroutine(AfterFireDamage());
        }

        // Ultimate damage
        else if (other.CompareTag("Ultimate") == true)
        {
            TakeDamage(playerScript.ultimateLevel * 2);

            Vector3 backDirection = (transform.position - backLandmark.position).normalized;
            rb.AddForce(-backDirection * playerScript.waveLevel * 4);
        }
    }

    private IEnumerator Raise()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("AttackInt", 0);

        animator.enabled = false;
        navMeshAgent.enabled = false;

        rb.AddForce(Vector3.up * playerScript.tornadoLevel * 35);

        /*
        float holdTime = playerScript.tornadoLevel / 5;
        float time = 0f;

        while (time < holdTime)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, 7.5f, transform.position.z), time / holdTime);

            time += Time.deltaTime;

            yield return null;
        }
        */

        yield return new WaitForSeconds(2.5f);
        animator.enabled = true;
        navMeshAgent.enabled = true;
    }

    private IEnumerator Freez(float seconds)
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("AttackInt", 0);

        animator.enabled = false;
        navMeshAgent.enabled = false;

        yield return new WaitForSeconds(seconds);
        animator.enabled = true;
        navMeshAgent.enabled = true;
    }

    private IEnumerator AfterFireDamage()
    {
        for (int i = 0; i < playerScript.fireLevel / 5; i++)
        {
            yield return new WaitForSeconds(.5f);
            TakeDamage(3);
        }
    }

    private void PlaceMarker()
    {
        // Instantiate marker on random position near by player radius (markerPlacementRadius)
        Vector3 randomDirection = Random.insideUnitSphere * markerPlacementRadius;
        Vector3 markerPosition = player.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(markerPosition, out hit, markerPlacementRadius, NavMesh.AllAreas))
        {
            marker = Instantiate(markerPrefab, hit.position, Quaternion.identity);
        }

    }

    private void AttackMarker()
    {
        if (marker != null)
        {
            damageCollider = Instantiate(damageColliderPrefab, marker.transform.position, Quaternion.identity);
            Destroy(marker, 0.1f);
        }
    }

}
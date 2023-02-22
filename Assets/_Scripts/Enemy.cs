using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    public int damage = 10;

    //private float minAttackRange = 3.75f;
    private float maxAttackRange = 4.25f;
    private float seeRange = 15f;

    private float attackRate = 4f;
    private float nextAttackTime;

    private float distance;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;
    private Skills playerScript;
    private Rigidbody rb;

    public Transform backLandmark;
    public Text elementText;

    public float markerPlacementRadius = 5f;

    private GameObject marker;
    public GameObject markerPrefab;
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
        playerScript = player.GetComponent<Skills>();

        material = GetComponentInChildren<Renderer>().material;
        int randomElement = Random.Range(0, 4);
        switch (randomElement)
        {
            case 0:
                element = Element.Air;
                material.SetColor("_EmissionColor", new Color(32, 32, 32) * 0.014f);
                elementText.text = "Air".ToString();
                elementText.color = Color.white;
                healthImage.color = Color.white;
                break;
            case 1:
                element = Element.Earth;
                material.SetColor("_EmissionColor", new Color(32, 27, 11) * 0.014f);
                elementText.text = "Earth".ToString();
                elementText.color = Color.gray;
                healthImage.color = Color.gray;
                break;
            case 2:
                element = Element.Fire;
                material.SetColor("_EmissionColor", new Color(32, 13, 11) * 0.014f);
                elementText.text = "Fire".ToString();
                elementText.color = Color.red;
                healthImage.color = Color.red;
                break;
            case 3:
                element = Element.Water;
                material.SetColor("_EmissionColor", new Color(11, 30, 32) * 0.014f);
                elementText.text = "Water".ToString();
                elementText.color = Color.cyan;
                healthImage.color = Color.cyan;
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

                Destroy(damageCollider);
                animator.SetBool("isAttacking", false);
            }

            if (Time.time >= nextAttackTime && distance < maxAttackRange)
            {
                PlaceMarker();

                #region Difference attack animations

                navMeshAgent.isStopped = true;

                int rand = Random.Range(1, 5);

                #endregion
                navMeshAgent.isStopped = true;
                animator.SetBool("isAttacking", true);

                nextAttackTime = Time.time + attackRate;
            }
            else if (distance >= maxAttackRange)
            {
                navMeshAgent.isStopped = false;
                animator.SetBool("isAttacking", false);
            }

        }

        #region UI set

        Vector3 cameraPosition = Camera.main.transform.position;
        healthBackground.transform.LookAt(cameraPosition);
        healthImage.transform.LookAt(cameraPosition);
        elementText.transform.LookAt(cameraPosition);
        healthImage.fillAmount = health / 100;

        #endregion

        if (health <= 0 || transform.position.y <= -10f)
        {
            playerScript.killsCounterInt++;
            animator.SetTrigger("Death");
            transform.GetComponent<Collider>().enabled = false;
            navMeshAgent.isStopped = true;
            rb.isKinematic = true;
            Destroy(gameObject, 4f);
            GetComponent<Enemy>().enabled = false;
        }

    }

    public void TakeDamageToHero()
    {
        player.GetComponent<Skills>().TakeDamage(damage);
    }

    private void LookAtTarget(GameObject target)
    {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void DeathE(string skillName)
    {
        // Death
        if (health <= 0 || transform.position.y <= -10f)
        {
            switch (skillName)
            {
                case "Meteor":
                    int MRP = PlayerPrefs.GetInt("Meteor relic progress");
                    MRP++;
                    PlayerPrefs.SetInt("Meteor relic progress", MRP);
                    break;
                case "Tornado":
                    int TRP = PlayerPrefs.GetInt("Tornado relic progress");
                    TRP++;
                    PlayerPrefs.SetInt("Tornado relic progress", TRP);
                    break;
                case "Fire":
                    int FRP = PlayerPrefs.GetInt("Fire relic progress");
                    FRP++;
                    PlayerPrefs.SetInt("Fire relic progress", FRP);
                    break;
                case "Wave":
                    int WRP = PlayerPrefs.GetInt("Wave relic progress");
                    WRP++;
                    PlayerPrefs.SetInt("Wave relic progress", WRP);
                    break;
                case "Ultimate":
                    int URP = PlayerPrefs.GetInt("Ultimate relic progress");
                    URP++;
                    PlayerPrefs.SetInt("Ultimate relic progress", URP);
                    break;
            }

        }
    }

    // Damage taken
    private void OnTriggerEnter(Collider other)
    {
        // ; element - zombie >> elements that match

        if (other.CompareTag("Meteor") == true)
        {
            if (element == Element.Water)
            {
                float randomDamage = Random.Range(2, 2.5f);
                TakeDamage(playerScript.meteorLevel * randomDamage);
            }
            else if (element != Element.Water)
            {
                float randomDamage = Random.Range(1.25f, 1.75f);
                TakeDamage(playerScript.meteorLevel * randomDamage);
            }

            DeathE("Meteor");
        }

        else if (other.CompareTag("Tornado") == true)
        {
            if (element == Element.Fire)
            {
                float randomDamage = Random.Range(2, 2.5f);
                TakeDamage(playerScript.tornadoLevel * randomDamage);
            }
            else if (element != Element.Fire)
            {
                float randomDamage = Random.Range(1.25f, 1.75f);
                TakeDamage(playerScript.tornadoLevel * randomDamage);
            }

            GameObject t = Instantiate(playerScript.visualTornado, transform.position, 
                Quaternion.LookRotation(Vector3.up));

            Destroy(t, 1.5f);

            StartCoroutine(Raise());
            DeathE("Tornado");
        }

        else if (other.CompareTag("Wave") == true)
        {
            if (element == Element.Earth)
            {
                float randomDamage = Random.Range(2, 2.5f);
                TakeDamage(playerScript.waveLevel * randomDamage);
            }
            else if (element != Element.Earth)
            {
                float randomDamage = Random.Range(1.25f, 1.75f);
                TakeDamage(playerScript.waveLevel * randomDamage);
            }

            Vector3 backDirection = (transform.position - backLandmark.position).normalized;
            rb.AddForce(-backDirection * playerScript.waveLevel * 16);
            StartCoroutine(Freez(playerScript.waveLevel / 12));
            DeathE("Wave");
        }

        else if (other.CompareTag("Fire") == true)
        {
            if (element == Element.Air)
            {
                float randomDamage = Random.Range(2, 2.5f);
                TakeDamage(playerScript.fireLevel * randomDamage);
            }
            else if (element != Element.Air)
            {
                float randomDamage = Random.Range(1.25f, 1.75f);
                TakeDamage(playerScript.fireLevel * randomDamage);
            }
            StartCoroutine(AfterFireDamage());
            DeathE("Fire");
        }

        // Ultimate damage
        else if (other.CompareTag("Ultimate") == true)
        {
            TakeDamage(playerScript.ultimateLevel * 2);

            Vector3 backDirection = (transform.position - backLandmark.position).normalized;
            rb.AddForce(-backDirection * playerScript.waveLevel * 4);
            DeathE("Ultimate");
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
            Destroy(marker, 2f);
        }

        // Set target position to marker
        navMeshAgent.SetDestination(marker.transform.position);

    }

    // Using only in animations
    private void AttackMarker()
    {
        if (marker != null)
        {
            damageCollider = Instantiate(damageColliderPrefab, marker.transform.position, Quaternion.identity);
            Destroy(marker, 0.1f);
        }
    }
    
    // Coroutines .
    private IEnumerator Raise()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("AttackInt", 0);

        animator.enabled = false;
        navMeshAgent.enabled = false;

        rb.AddForce(Vector3.up * playerScript.tornadoLevel * 35);

        yield return new WaitForSeconds(playerScript.tornadoLevel / 7.5f);
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
            TakeDamage(playerScript.fireLevel / 7);
        }
    }

}
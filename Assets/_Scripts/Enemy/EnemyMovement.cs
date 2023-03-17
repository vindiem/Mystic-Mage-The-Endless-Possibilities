using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyUI enemyUI;
    private EnemyAttackSystem enemyAttackSystem;

    public float health = 100;
    [HideInInspector] public float maxHealth;
    private bool hasDead = false;

    [HideInInspector] public float distance;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [HideInInspector] public Animator animator;
    private Transform player;
    [HideInInspector] public Skills playerScript;
    [HideInInspector] public Rigidbody rb;

    public Transform backLandmark;

    // What element did the zombie die from
    [HideInInspector] public bool ultimateDeath = false;
    [HideInInspector] public bool meteorDeath = false;
    [HideInInspector] public bool tornadoDeath = false;
    [HideInInspector] public bool fireDeath = false;
    [HideInInspector] public bool waveDeath = false;

    private LevelCoins levelCoins;
    public GameObject coinPrefab;

    public enum Element
    {
        Earth,
        Air,
        Fire,
        Water
    };
    private Element element;
    private Material material;

    [Header("Sounds")]
    [HideInInspector] public AudioSource m_audioSource;
    public AudioClip deathSound;
    public AudioClip attackSound;
    [HideInInspector] public AudioSource runningSource;

    private void Start()
    {
        enemyAttackSystem = GetComponent<EnemyAttackSystem>();
        enemyUI = GetComponent<EnemyUI>();

        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        levelCoins = GameObject.FindGameObjectWithTag("LevelCoins").GetComponent<LevelCoins>();

        player = GameObject.FindWithTag("Player").transform;
        playerScript = player.GetComponent<Skills>();

        material = GetComponentInChildren<Renderer>().material;
        int randomElement = Random.Range(0, 4);
        switch (randomElement)
        {
            case 0:
                element = Element.Air;
                material.SetColor("_EmissionColor", new Color(32, 32, 32) * 0.014f);
                enemyUI.elementText.text = "Air".ToString();
                enemyUI.elementText.color = Color.white;
                enemyUI.healthImage.color = Color.white;
                break;
            case 1:
                element = Element.Earth;
                material.SetColor("_EmissionColor", new Color(32, 27, 11) * 0.014f);
                enemyUI.elementText.text = "Earth".ToString();
                enemyUI.elementText.color = Color.gray;
                enemyUI.healthImage.color = Color.gray;
                break;
            case 2:
                element = Element.Fire;
                material.SetColor("_EmissionColor", new Color(32, 13, 11) * 0.014f);
                enemyUI.elementText.text = "Fire".ToString();
                enemyUI.elementText.color = Color.red;
                enemyUI.healthImage.color = Color.red;
                break;
            case 3:
                element = Element.Water;
                material.SetColor("_EmissionColor", new Color(11, 30, 32) * 0.014f);
                enemyUI.elementText.text = "Water".ToString();
                enemyUI.elementText.color = Color.cyan;
                enemyUI.healthImage.color = Color.cyan;
                break;
        }

        // Audio source
        m_audioSource = GameObject.FindGameObjectWithTag("Sounds").GetComponent<AudioSource>();
        runningSource = GameObject.FindGameObjectWithTag("Zombie running").GetComponent<AudioSource>();
        runningSource.mute = true;

        #region Health set based on player level

        int playerLevel = PlayerPrefs.GetInt("Level");
        float playerLevelFloat = Random.Range(playerLevel - 0.5f, playerLevel + 0.5f);
        health *= playerLevelFloat / 10;
        maxHealth = health;

        #endregion

    }

    private void Update()
    {
        // Do nothing if player destroyed or haven't spawned or when navMeshAgent isActive -> false
        if (player == null || navMeshAgent.enabled == false) return;

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        #region Set float distance

        if (enemyAttackSystem != null)
        {
            // if marker is instantiate -> target - marker
            // if marker isn't instantiate -> target - player
            if (enemyAttackSystem.marker == null)
            {
                distance = Vector3.Distance(transform.position, player.position);
            }
            else if (enemyAttackSystem.marker != null)
            {
                distance = Vector3.Distance(transform.position, enemyAttackSystem.marker.transform.position);
                LookAtTarget(enemyAttackSystem.marker);
            }
        }
        else
        {
            distance = Vector3.Distance(transform.position, player.position);
        }

        #endregion

        // Death
        if ((health <= 0 && hasDead == false) || transform.position.y < -10f)
        {
            hasDead = true;

            // Experience
            int randomXP = Random.Range(40, 80);
            float currentXP = PlayerPrefs.GetFloat("Xp");
            currentXP += randomXP;
            PlayerPrefs.SetFloat("Xp", currentXP);

            // Coin instantiate by chance 20%
            int coinAddChance = Random.Range(0, 5);
            if (coinAddChance == 0)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x,
                    transform.position.y + 2f, transform.position.z);
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            }

            playerScript.killsCounterInt++;
            levelCoins.CheckLevel();
            m_audioSource.PlayOneShot(deathSound);
            animator.SetTrigger("Death");

            // Visual death
            transform.GetComponent<Collider>().enabled = false;
            navMeshAgent.isStopped = true;
            rb.isKinematic = true;

            Destroy(gameObject, 4f);
            transform.GetComponent<EnemyMovement>().enabled = false;
            if (enemyAttackSystem != null && enemyUI != null)
            {
                transform.GetComponent<EnemyAttackSystem>().enabled = false;
                transform.GetComponent<EnemyUI>().enabled = false;
            }

        }

    }

    // Damage taken
    private void OnTriggerEnter(Collider other)
    {
        // ; element - zombie >> elements that match

        // Meteor > Water
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

            enemyUI.RelicKillUpdate("Meteor");
        }

        // Tornado > Fire
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
            enemyUI.RelicKillUpdate("Tornado");
        }

        // Wave > Earth
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
            enemyUI.RelicKillUpdate("Wave");
        }

        // Fire > Air
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
            enemyUI.RelicKillUpdate("Fire");
        }

        // Ultimate damage
        else if (other.CompareTag("Ultimate") == true)
        {
            TakeDamage(playerScript.ultimateLevel * 2);

            Vector3 backDirection = (transform.position - backLandmark.position).normalized;
            rb.AddForce(-backDirection * playerScript.waveLevel * 4);
            enemyUI.RelicKillUpdate("Ultimate");
        }

    }

    private void LookAtTarget(GameObject target)
    {
        transform.LookAt(new Vector3(target.transform.position.x,
            transform.position.y, target.transform.position.z));
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        // If in the future there will be something that can be done after causing damage to zombies,
        // then it will be possible to register it here
    }

    // Coroutines
    private IEnumerator Raise()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("isAttacking", false);
        animator.SetInteger("AttackInt", 0);

        animator.enabled = false;
        navMeshAgent.enabled = false;

        rb.AddForce(Vector3.up * 650);

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
            TakeDamage(playerScript.fireLevel / 8);
        }
    }

}

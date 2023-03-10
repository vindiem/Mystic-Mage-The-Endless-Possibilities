using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    private float maxHealth;
    private bool hasDead = false;

    private int EditedDamage = 10;
    public int Damage = 10;

    private float maxAttackRange = 4.25f;
    private float seeRange = 15f;

    private float attackRate = 4f;
    private float nextAttackTime;

    private float distance;
    [HideInInspector] public NavMeshAgent navMeshAgent;
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

    // What element did the zombie die from
    private bool ultimateDeath = false;
    private bool meteorDeath = false;
    private bool tornadoDeath = false;
    private bool fireDeath = false;
    private bool waveDeath = false;

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

    // Health imagine
    public Image healthBackground;
    public Image healthImage;
    public Text healthText;

    // Relics
    private Animator relicPlate;

    [Header("Sounds")]
    private AudioSource m_audioSource;
    public AudioClip deathSound;
    public AudioClip attackSound;
    private AudioSource runningSource;

    private void Start()
    {
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

        // Relic A
        relicPlate = GameObject.FindGameObjectWithTag("RelicPlate").GetComponent<Animator>();

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
        // Do nothing if player destroyed or havn't spawned or when navMeshAgent isActive -> false
        if (player == null || navMeshAgent.enabled == false) return;

        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        // Randomize Damage
        Damage = Random.Range(EditedDamage - 4, EditedDamage + 3);
        
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
            }

            if (Time.time >= nextAttackTime && distance < maxAttackRange)
            {
                // Running sound
                runningSource.mute = true;

                // Instantiate marker
                PlaceMarker();

                #region Difference attack animations

                navMeshAgent.isStopped = true;

                int rand = Random.Range(1, 5);
                animator.SetInteger("AttackInt", rand);
                animator.SetBool("isAttacking", true);
                StartCoroutine(AttackOff());

                #endregion

                // Attack sound play
                m_audioSource.PlayOneShot(attackSound);

                // Sey next time attack, so that the zombie doesn't hit the player every time
                nextAttackTime = Time.time + attackRate;

            }
            else if (distance > maxAttackRange)
            {
                navMeshAgent.isStopped = false;

                // Running sound
                runningSource.mute = false;
            }

        }

        #region UI set

        Vector3 cameraPosition = Camera.main.transform.position;

        healthBackground.transform.LookAt(cameraPosition);
        healthImage.transform.LookAt(cameraPosition);
        elementText.transform.LookAt(cameraPosition);
        healthText.transform.LookAt(cameraPosition);

        healthImage.fillAmount = health / maxHealth;
        healthText.text = $"{(int)health} / {(int)maxHealth}";

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
            transform.GetComponent<Enemy>().enabled = false;

        }

    }

    private void LookAtTarget(GameObject target)
    {
        transform.LookAt(new Vector3(target.transform.position.x, 
            transform.position.y, target.transform.position.z));
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        // If in the future there will be something that can be done after causing damage to zombies,
        // then it will be possible to register it here
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

            RelicKillUpdate("Meteor");
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
            RelicKillUpdate("Tornado");
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
            RelicKillUpdate("Wave");
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
            RelicKillUpdate("Fire");
        }

        // Ultimate damage
        else if (other.CompareTag("Ultimate") == true)
        {
            TakeDamage(playerScript.ultimateLevel * 2);

            Vector3 backDirection = (transform.position - backLandmark.position).normalized;
            rb.AddForce(-backDirection * playerScript.waveLevel * 4);
            RelicKillUpdate("Ultimate");
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
            navMeshAgent.SetDestination(marker.transform.position);
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

    private IEnumerator AttackOff()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("isAttacking", false);
    }

    // Relics
    private void RelicAchievement(int relicLevel, string nameOfRelic)
    {
        string MeteorBlock,
            TornadoBlock,
            FireBlock,
            WaveBlock,
            UltimateBlock;

        MeteorBlock = PlayerPrefs.GetString("MeteorBlock");
        TornadoBlock = PlayerPrefs.GetString("TornadoBlock");
        FireBlock = PlayerPrefs.GetString("FireBlock");
        WaveBlock = PlayerPrefs.GetString("WaveBlock");
        UltimateBlock = PlayerPrefs.GetString("UltimateBlock");

        switch (nameOfRelic)
        {
            case "Meteor":
                if (MeteorBlock == "false")
                {
                    float mmrp = PlayerPrefs.GetFloat("mMRP");

                    if (relicLevel > mmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New meteor relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)mmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        mmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        mmrp = 1;
                    }

                    PlayerPrefs.SetFloat("mMRP", mmrp);
                }
                break;
            case "Tornado":
                if (TornadoBlock == "false")
                {
                    float tmrp = PlayerPrefs.GetFloat("mTRP");
                    if (relicLevel > tmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New tornado relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)tmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        tmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        tmrp = 1;
                    }
                    PlayerPrefs.SetFloat("mTRP", tmrp);
                }
                break;
            case "Fire":
                if (FireBlock == "false")
                {
                    float fmrp = PlayerPrefs.GetFloat("mFRP");
                    if (relicLevel > fmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New fire relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)fmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        fmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        fmrp = 1;
                    }
                    PlayerPrefs.SetFloat("mFRP", fmrp);
                }
                break;
            case "Wave":
                if (WaveBlock == "false")
                {
                    float wmrp = PlayerPrefs.GetFloat("mWRP");
                    if (relicLevel > wmrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New wave relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)wmrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        wmrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        wmrp = 1;
                    }
                    PlayerPrefs.SetFloat("mWRP", wmrp);
                }
                break;
            case "Ultimate":
                if (UltimateBlock == "false")
                {
                    float umrp = PlayerPrefs.GetFloat("mURP");
                    if (relicLevel > umrp && relicLevel <= 1280)
                    {
                        Text relicPlateText = relicPlate.GetComponentInChildren<Text>();
                        relicPlateText.text = "New ULTIMATE relic reached";
                        relicPlate.SetTrigger("On");

                        int coinsCount = PlayerPrefs.GetInt("Coins");
                        coinsCount += (int)umrp;
                        PlayerPrefs.SetInt("Coins", coinsCount);

                        umrp *= 2;
                    }
                    else if (relicLevel > 1280)
                    {
                        umrp = 1;
                    }
                    PlayerPrefs.SetFloat("mURP", umrp);
                }
                break;
        }
    }

    private void RelicKillUpdate(string skillName)
    {
        // If death check if relic has been reached
        if (health <= 0 || transform.position.y <= -10f)
        {
            string MeteorBlock,
                TornadoBlock,
                FireBlock,
                WaveBlock,
                UltimateBlock;

            MeteorBlock = PlayerPrefs.GetString("MeteorBlock");
            TornadoBlock = PlayerPrefs.GetString("TornadoBlock");
            FireBlock = PlayerPrefs.GetString("FireBlock");
            WaveBlock = PlayerPrefs.GetString("WaveBlock");
            UltimateBlock = PlayerPrefs.GetString("UltimateBlock");

            switch (skillName)
            {
                case "Meteor":
                    if (meteorDeath == false && MeteorBlock == "false")
                    {
                        meteorDeath = true;
                        int MRP = PlayerPrefs.GetInt("Meteor relic progress");
                        MRP++;
                        RelicAchievement(MRP, "Meteor");
                        PlayerPrefs.SetInt("Meteor relic progress", MRP);
                    }
                    break;
                case "Tornado":
                    if (tornadoDeath == false && TornadoBlock == "false")
                    {
                        tornadoDeath = true;
                        int TRP = PlayerPrefs.GetInt("Tornado relic progress");
                        TRP++;
                        RelicAchievement(TRP, "Tornado");
                        PlayerPrefs.SetInt("Tornado relic progress", TRP);
                    }
                    break;
                case "Fire":
                    if (fireDeath == false && FireBlock == "false")
                    {
                        fireDeath = true;
                        int FRP = PlayerPrefs.GetInt("Fire relic progress");
                        FRP++;
                        RelicAchievement(FRP, "Fire");
                        PlayerPrefs.SetInt("Fire relic progress", FRP);
                    }
                    break;
                case "Wave":
                    if (waveDeath == false && WaveBlock == "false")
                    {
                        waveDeath = true;
                        int WRP = PlayerPrefs.GetInt("Wave relic progress");
                        WRP++;
                        RelicAchievement(WRP, "Wave");
                        PlayerPrefs.SetInt("Wave relic progress", WRP);
                    }
                    break;
                case "Ultimate":
                    if (ultimateDeath == false && UltimateBlock == "false")
                    {
                        ultimateDeath = true;
                        int URP = PlayerPrefs.GetInt("Ultimate relic progress");
                        URP++;
                        RelicAchievement(URP, "Ultimate");
                        PlayerPrefs.SetInt("Ultimate relic progress", URP);
                    }
                    break;
            }

        }
    }

}
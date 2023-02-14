using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [HideInInspector] public Movement movementScript;

    public Text killsCounter;
    [HideInInspector] public int killsCounterInt;

    // Vectors
    private Vector3 currentMousePosition = new Vector3();

    // Charavter variables
    private float health = 100;

    [Header("Items")]
    public Transform fireLandmark;
    public Transform meteorLandmark;
    public Transform waveLandmark;

    public GameObject fire;
    public int fireLevel = 2;

    public GameObject wave;
    public int waveLevel = 3;

    public GameObject tornado;
    public GameObject visualTornado;
    public int tornadoLevel = 5;

    public GameObject meteor;
    public int meteorLevel = 4;

    public GameObject ultimate;
    public int ultimateLevel = 4;
    public Transform[] directions;

    [Header("SkillsButtons")]
    public Image[] iconButtons;

    // Skills k/d's
    private List<float> kds = new List<float>();
    private float[] ckds = new float[5];

    // Z - 0 [Fire] - Fire
    // X - 1 [Wave] - Water
    // C - 2 [Tornado] - Air
    // V - 3 [Meteor] - Earth
    // SPACE - 4 [Ultimate]

    // 5 elements

    // UI Elements
    public Image healthImage;

    private void Start()
    {
        movementScript = GetComponent<Movement>();

        // Make KDs array full
        for (int i = 0; i <= 4; i++)
        {
            kds.Add(0);
        }

        Time.timeScale = 1f;
    }

    private void Update()
    {
        #region KD & Buttons images fill

        for (int i = 0; i < kds.Count; i++)
        {
            if (kds[i] > 0)
            {
                float fill = kds[i] * 100 / ckds[i] / 100;
                iconButtons[i].fillAmount = 1 - fill;
                kds[i] -= Time.deltaTime;
            }
        }

        #endregion

        #region Skills using

        // look to mouse position
        if (Input.GetKey(KeyCode.F) == true)
        {
            movementScript.RotateToMouse();
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Fire();
        }

        else if (Input.GetKeyDown(KeyCode.X))
        {
            Wave();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            Tornado();
        }

        else if (Input.GetKeyDown(KeyCode.V))
        {
            Meteor();
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Ultimate();
        }

        #endregion

        #region UI

        healthImage.fillAmount = health / 100;
        killsCounter.text = killsCounterInt.ToString();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        #endregion

    }

    // Z, X, C, V, SPACE
    // Jump [SHIFT]

    // [Z] Fire / Dragon Knight ;
    public void Fire()
    {
        if (kds[0] <= 0)
        {
            Vector3 direction = (fireLandmark.position - transform.position).normalized;

            GameObject f = Instantiate(fire, new Vector3(transform.position.x, transform.position.y + 3.5f, 
                transform.position.z), Quaternion.LookRotation(direction));

            ParticleSystem.MainModule fireParticleSystem = f.GetComponent<ParticleSystem>().main;
            float lifetime = fireLevel / 10;
            fireParticleSystem.startLifetime = (float)lifetime;

            Destroy(f, fireLevel / 3);

            kds[0] = 4;
            SetCkds(kds[0], 0);
        }
    }

    // [X] Wave / Tide ;
    public void Wave()
    {
        if (kds[1] <= 0)
        {
            Vector3 direction = (waveLandmark.position - transform.position).normalized;

            GameObject w = Instantiate(wave, new Vector3(transform.position.x,
                transform.position.y + 1.5f, transform.position.z), Quaternion.LookRotation(direction));

            Rigidbody wrb = w.GetComponent<Rigidbody>();
            wrb.AddForce(direction * waveLevel * 16);

            Destroy(w, waveLevel / 8);

            kds[1] = 6f;
            SetCkds(kds[1], 1);
        }
    }

    // [C] Tornato / Invoker ;
    public void Tornado()
    {
        if (kds[2] <= 0)
        {
            GameObject t = Instantiate(tornado, transform.position, Quaternion.LookRotation(Vector3.up));

            Vector3 direction = (t.transform.position - currentMousePosition).normalized;
            t.GetComponent<Rigidbody>().AddForce(-direction * tornadoLevel * 16);

            Destroy(t, tornadoLevel / 7.5f);

            kds[2] = 7;
            SetCkds(kds[2], 2);
        }
    }

    // [V] Chaos Meteor / Invoker ;
    public void Meteor()
    {
        if (kds[3] <= 0)
        {
            /*GameObject m = Instantiate(meteor, new Vector3(transform.position.x,
                transform.position.y + 8f, transform.position.z), Quaternion.identity);*/

            movementScript.RotateToMouse();
            GameObject m = Instantiate(meteor, meteorLandmark.position, Quaternion.identity);

            float distance = Vector3.Distance(transform.position, currentMousePosition);
            float modifiedForce = meteorLevel / 8 * distance;

            Vector3 direction = (transform.position - currentMousePosition).normalized;
            m.GetComponent<Rigidbody>().AddForce(-direction * modifiedForce, ForceMode.Impulse);

            Destroy(m, meteorLevel / 2);

            kds[3] = 3.5f;
            SetCkds(kds[3], 3);
        }
    }

    // [SPACE] Ultimate / Invoker ;
    public void Ultimate()
    {
        if (kds[4] <= 0)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 direction = (directions[i].position - transform.position).normalized;

                GameObject u = Instantiate(ultimate, new Vector3(transform.position.x,
                    transform.position.y + 1.5f, transform.position.z), Quaternion.LookRotation(direction));

                Rigidbody urb = u.GetComponent<Rigidbody>();
                urb.AddForce(direction * ultimateLevel * 32);

                Destroy(u, ultimateLevel / 8);
            }

            kds[4] = 7;
            SetCkds(kds[4], 4);
        }
    }


    // const kds
    private void SetCkds(float kdi, int i)
    {
        ckds[i] = kdi;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHit") == true)
        {
            TakeDamage(enemy.damage);
        }
    }

}

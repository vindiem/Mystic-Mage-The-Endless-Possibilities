using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skills : MonoBehaviour
{
    // Scripts
    [SerializeField] private Enemy enemy;
    [HideInInspector] public Movement movementScript;

    // Kills counter
    public Text killsCounter;
    [HideInInspector] public int killsCounterInt;

    // Vectors
    private Vector3 currentMousePosition = new Vector3();

    // Charavter variables
    private float health = 100;
    public float gameSpeed = 1.25f;

    public LayerMask CastLayer;
    private Joystick attackJoystick;

    [Header("Items")]
    // Landmarks
    public Transform fireLandmark;
    public Transform meteorLandmark;
    public Transform waveLandmark;

    // Objects || Prefabs
    public GameObject fire;
    public int fireLevel = 2;

    public GameObject wave;
    public int waveLevel = 3;

    public GameObject tornado;
    public GameObject visualTornado;
    public int tornadoLevel = 5;
    private bool canTornadoInvoke = true;

    public GameObject meteor;
    public int meteorLevel = 4;
    private bool canMeteorInvoke = true;

    public GameObject ultimate;
    public int ultimateLevel = 4;
    public Transform[] directions;

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

    [Header("SkillsButtons")]
    // PC
    [SerializeField] private Image[] iconButtons;
    [SerializeField] private GameObject PCSkills;
    [SerializeField] private RawImage[] bgsP;

    // Mobile
    [SerializeField] private Image[] iconButtonsMobile;
    [SerializeField] private GameObject MobileSkills;
    [SerializeField] private RawImage[] bgsM; 

    private void Start()
    {
        movementScript = GetComponent<Movement>();

        switch (movementScript.movementType)
        {
            default:
                for (int i = 0; i < iconButtonsMobile.Length; i++)
                {
                    iconButtons[i].gameObject.SetActive(true);
                    iconButtonsMobile[i].gameObject.SetActive(false);
                }
                PCSkills.SetActive(true);
                MobileSkills.SetActive(false);
                break;

            case Movement.MovementType.Mobile:
                for (int i = 0; i < iconButtonsMobile.Length; i++)
                {
                    iconButtons[i].gameObject.SetActive(false);
                    iconButtonsMobile[i].gameObject.SetActive(true);
                }
                PCSkills.SetActive(false);
                MobileSkills.SetActive(true);
                break;
        }

        // Make KDs array full
        for (int i = 0; i <= 4; i++)
        {
            kds.Add(0);
        }

        // Visual
        for (int i = 0; i < kds.Count; i++)
        {
            bgsP[i].color = Color.grey;
            bgsM[i].color = Color.grey;
        }

        // Game speed
        Time.timeScale = 1.25f;

        attackJoystick = movementScript.attackJoystick;

    }

    private void Update()
    {

        #region Skills using

        // Mobile skills using
        if (movementScript.movementType == Movement.MovementType.Mobile)
        {
            float HorizontalAxis = attackJoystick.Horizontal;
            float VerticalAxis = attackJoystick.Vertical;

            if (VerticalAxis > 0.4f && HorizontalAxis < -0.4f)
            {
                Fire();
            }
            else if (HorizontalAxis < -0.8f)
            {
                TornadoInvoke();
            }
            else if (VerticalAxis > 0.4f && HorizontalAxis > 0.4f)
            {
                Wave();
            }
            else if (HorizontalAxis > 0.8f)
            {
                MeteorInvoke();
            }
            else if (VerticalAxis < -0.8f)
            {
                Ultimate();
            }

            // Make skills icond visible
            if (HorizontalAxis > 0.1f || VerticalAxis > 0.1f ||
                HorizontalAxis < -0.1f || VerticalAxis < -0.1f)
            {
                MobileSkills.gameObject.SetActive(true);
            }
            else
            {
                MobileSkills.gameObject.SetActive(false);
            }

        }

        // PC skills using
        else
        {
            // look to mouse position
            if (Input.GetKey(KeyCode.F) == true)
            {
                movementScript.RotateToMouse(currentMousePosition, true);
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
                TornadoInvoke();
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                MeteorInvoke();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Ultimate();
            }
        }

        #endregion

        #region UI

        // KD & Buttons images fill
        for (int i = 0; i < kds.Count; i++)
        {
            if (kds[i] > 0)
            {
                float fill = kds[i] * 100 / ckds[i] / 100;
                iconButtons[i].fillAmount = 1 - fill;
                iconButtonsMobile[i].fillAmount = 1 - fill;
                kds[i] -= Time.deltaTime;
            }
        }

        // Health
        healthImage.fillAmount = health / 100;
        killsCounter.text = killsCounterInt.ToString("000");

        if (health <= 0 || transform.position.y <= -10f)
        {
            Destroy(gameObject);
        }

        #endregion

        #region Get current mouse (touch) position

        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseWorldPosition, out RaycastHit raycastHit, CastLayer))
        {
            currentMousePosition = raycastHit.point;
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
    public void TornadoInvoke()
    {
        if (canTornadoInvoke == true)
        {
            StartCoroutine(Tornado());
        }
    }
    public IEnumerator Tornado()
    {
        if (kds[2] <= 0)
        {
            #region Visual
            bgsM[2].color = Color.green;
            bgsP[2].color = Color.green;
            canTornadoInvoke = false;
            #endregion

            // Wait until left mouse button will be pressed (Trigger)
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            movementScript.RotateToMouse(currentMousePosition, true);
            GameObject t = Instantiate(tornado, transform.position, Quaternion.LookRotation(Vector3.up));

            Vector3 direction = (t.transform.position - currentMousePosition).normalized;
            t.GetComponent<Rigidbody>().AddForce(-direction * tornadoLevel * 16);

            Destroy(t, tornadoLevel / 3.5f);

            kds[2] = 7;
            SetCkds(kds[2], 2);

            #region Visual
            bgsM[2].color = Color.grey;
            bgsP[2].color = Color.grey;
            canTornadoInvoke = true;
            #endregion
        }
    }

    // [V] Chaos Meteor / Invoker ;
    public void MeteorInvoke()
    {
        if (canMeteorInvoke == true)
        {
            StartCoroutine(Meteor());
        }
    }
    public IEnumerator Meteor()
    {
        if (kds[3] <= 0)
        {
            #region Visual
            bgsM[3].color = Color.green;
            bgsP[3].color = Color.green;
            canMeteorInvoke = false;
            #endregion

            // Wait until left mouse button (touch) will be pressed (Trigger)
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            movementScript.RotateToMouse(currentMousePosition, true);
            GameObject m = Instantiate(meteor, meteorLandmark.position, Quaternion.identity);

            float distance = Vector3.Distance(transform.position, currentMousePosition);
            float modifiedForce = meteorLevel / 8 * distance;

            Vector3 direction = (transform.position - currentMousePosition).normalized;
            m.GetComponent<Rigidbody>().AddForce(-direction * modifiedForce, ForceMode.Impulse);

            Destroy(m, meteorLevel / 2);

            kds[3] = 3.5f;
            SetCkds(kds[3], 3);

            #region Visual
            bgsM[3].color = Color.grey;
            bgsP[3].color = Color.grey;
            canMeteorInvoke = true;
            #endregion
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OutVoker : MonoBehaviour
{
    public LayerMask CastLayer;
    private Rigidbody rb;

    // Vectors
    private Vector3 currentMousePosition = new Vector3();
    private Vector3 targetPosition = new Vector3();
    private Quaternion lookRotation;

    [Header("Charavter variables")]
    private float health = 100;
    private int movementSpeed = 6;
    private int rotationSpeed = 10;
    private Animator animator;
    
    // Jump variables
    private bool onGround = false;
    private float distanceToGround = 0.5f;
    private float jumpForce = 7.6f;

    [Header("Items")]
    public GameObject mouseEffect;
    public Transform landmark;

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
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Make KDs array full
        for (int i = 0; i <= 4; i++)
        {
            kds.Add(0);
        }

    }

    private void Update()
    {
        #region Get current mouse position

        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseWorldPosition, out RaycastHit raycastHit, CastLayer))
        {
            currentMousePosition = raycastHit.point;
        }

        #endregion

        #region Set target position

        // ... & set target position (if right mouse button has pressed)
        if (Input.GetMouseButtonDown(1))
        {
            // Set look rotation
            lookRotation = Quaternion.LookRotation(new Vector3(currentMousePosition.x - transform.position.x, 
                0, currentMousePosition.z - transform.position.z));

            targetPosition = currentMousePosition;

            // Instantiate mouse effect
            GameObject effect = Instantiate(mouseEffect, new Vector3(currentMousePosition.x, currentMousePosition.y + 0.1f,
                currentMousePosition.z), Quaternion.LookRotation(Vector3.up));
        }

        #endregion

        #region Movement relatively target position

        Vector3 transformPositionXZ = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 targetPositionXZ = new Vector3(targetPosition.x, 0f, targetPosition.z);

        if (transformPositionXZ != targetPositionXZ)
        {
            // Rotating to target position
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Moving to target position
            targetPositionXZ.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, targetPositionXZ, movementSpeed * Time.deltaTime);

            // Animations
            animator.SetBool("isRunning", true);

        }
        else
        {
            // Animations
            animator.SetBool("isRunning", false);

        }

        #endregion

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

        #region Jump

        onGround = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

        if (Input.GetKeyDown(KeyCode.LeftShift) && onGround == true)
        {
            rb.velocity = Vector3.up * jumpForce;
        }

        #endregion

        #region Skills using

        // look to mouse position
        if (Input.GetKey(KeyCode.F) == true)
        {
            RotateToMouse();
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

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        #endregion

    }

    // Z, X, C, V, SPACE
    // Jump [SHIFT]

    // {F}
    public void RotateToMouse()
    {
        // Look At transform main character with his own axis (y) and target position (x, z)
        // In 2D (x, z) = (x, y)
        transform.LookAt(new Vector3(currentMousePosition.x, transform.position.y, currentMousePosition.z));

        // Set target position to player position to stop character
        targetPosition = transform.position;
    }

    // [Z] Fire / Dragon Knight ;
    public void Fire()
    {
        if (kds[0] <= 0)
        {
            Vector3 direction = (landmark.position - transform.position).normalized;
            RotateToMouse();

            GameObject f = Instantiate(fire, new Vector3(transform.position.x, transform.position.y + 4f, 
                transform.position.z), Quaternion.LookRotation(direction));

            Destroy(f, fireLevel / 3);

            kds[0] = 4;
            SetCkds(kds[0], 0);
        }
    }

    // [X] Wave / Tide
    public void Wave()
    {
        if (kds[1] <= 0)
        {
            kds[1] = 6f;
            SetCkds(kds[1], 1);
        }
    }

    // [C] Tornato / Invoker ;
    public void Tornado()
    {
        if (kds[2] <= 0)
        {
            GameObject t = Instantiate(tornado, new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                Quaternion.LookRotation(Vector3.up));

            Vector3 direction = (t.transform.position - currentMousePosition).normalized;
            t.GetComponent<Rigidbody>().AddForce(-direction * tornadoLevel * 32);

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
            GameObject m = Instantiate(meteor, new Vector3(transform.position.x,
                transform.position.y + 8f, transform.position.z), Quaternion.identity);

            float distance = Vector3.Distance(transform.position, currentMousePosition);
            float modifiedForce = meteorLevel / 2 * distance;

            Vector3 direction = (transform.position - currentMousePosition).normalized;
            m.GetComponent<Rigidbody>().AddForce(-direction * modifiedForce, ForceMode.Impulse);

            Destroy(m, meteorLevel / 2);

            kds[3] = 3.5f;
            SetCkds(kds[3], 3);
        }
    }

    // [SPACE] Ultimate / Invoker
    public void Ultimate()
    {
        if (kds[4] <= 0)
        {
            kds[4] = 4;
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

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutVoker : MonoBehaviour
{
    public LayerMask CastLayer;
    //private Rigidbody rigidbody;

    // Vectors
    private Vector3 currentMousePosition = new Vector3();
    private Vector3 targetPosition = new Vector3();
    private Quaternion lookRotation;

    [Header("Charavter variabels")]
    private int movementSpeed = 6;
    private int rotationSpeed = 10;
    private Animator animator;
    public int force = 0;

    [Header("Items")]
    public GameObject mouseEffect;
    public GameObject meteor;

    // Skills k/d's
    private List<float> KDs = new List<float>();

    // TAB - 0

    // Q - 1++
    // W - 2
    // E - 3
    // R - 4
    // T - 5
    // Y - 6

    // A - 7++
    // S - 8
    // D - 9
    // F - 10--
    // G - 11++
    // H - 12

    // Z - 13
    // X - 14
    // C - 15
    // V - 16
    // B - 17
    // N - 18

    private void Start()
    {
        // Make KDs array full
        for (int i = 0; i <= 18; i++)
        {
            KDs.Add(0);
        }

        //rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        #region Get current mouse position

        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseWorldPosition, out RaycastHit raycastHit, CastLayer))
        {
            currentMousePosition = raycastHit.point;
        }
        currentMousePosition.y = 0;

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
            GameObject effect = Instantiate(mouseEffect, new Vector3(currentMousePosition.x, currentMousePosition.y + 0.51f, 
                currentMousePosition.z), Quaternion.LookRotation(Vector3.up));
        }

        #endregion

        RotateToMouse();

        Dash();
        Meteor();
        Arrows();

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

        #region KD
        
        for (int i = 0; i < KDs.Count; i++)
        {
            if (KDs[i] > 0)
            {
                KDs[i] -= Time.deltaTime;
            }
        }
        
        #endregion

    }

    // [Q], W, E, R, T, Y
    // [A], S, D, {F}, [G], H
    // Z, X, C, V, B, N
    // TAB

    // [Q]
    private void Arrows()
    {
        if (Input.GetKeyDown(KeyCode.Q) && KDs[1] <= 0)
        {

        }
    }

    // [A]
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.A) && KDs[7] <= 0)
        {
            transform.Translate(Vector3.forward * force);
            targetPosition = transform.position;

            KDs[7] = 10;
        }
    }

    // [F]
    private void RotateToMouse()
    {
        // look to mouse position
        if (Input.GetKey(KeyCode.F) == true)
        {
            // Look At transform main character with his own axis (y) and target position (x, z)
            // In 2D (x, z) = (x, y)
            transform.LookAt(new Vector3(currentMousePosition.x, transform.position.y, currentMousePosition.z));

            // Set target position to player position to stop character
            targetPosition = transform.position;

        }
    }

    // [G]
    private void Meteor()
    {
        if (Input.GetKeyDown(KeyCode.G) && KDs[11] <= 0)
        {
            GameObject m = Instantiate(meteor, new Vector3(currentMousePosition.x, 
                transform.position.y + 9f, currentMousePosition.z), Quaternion.identity);

            KDs[11] = 20;
        }
    }

}


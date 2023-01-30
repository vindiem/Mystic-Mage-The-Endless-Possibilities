using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OutVoker : MonoBehaviour
{
    public LayerMask CastLayer;
    private Vector3 currentMousePosition = new Vector3();

    private Vector3 targetPosition = new Vector3();
    private Quaternion lookRotation;

    // character variables
    private int movementSpeed = 6;
    private int rotationSpeed = 10;
    private Animator animator;

    private void Start()
    {
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
        }

        #endregion

        RotateToMouse();

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

    }

    private void RotateToMouse()
    {
        // look to mouse position
        if (Input.GetKey(KeyCode.F) == true)
        {
            // Look At transform main character with his own axis (y) and target position (x, z)
            // In 2D (x, z) = (x, y)
            transform.LookAt(new Vector3(currentMousePosition.x, transform.position.y, currentMousePosition.z));

            // Set target position to player position to stop caracter
            targetPosition = transform.position;

        }
    }

}


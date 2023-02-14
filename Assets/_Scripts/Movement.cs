using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum MovemntType
    {
        Keyboard,
        Mouse,
        Mobile
    }
    [SerializeField] private MovemntType movementType;

    public LayerMask CastLayer;
    private Animator animator;
    private Rigidbody rb;

    // Vectors
    private Vector3 currentMousePosition = new Vector3();
    private Vector3 targetPosition = new Vector3();
    private Quaternion lookRotation;

    // Jump variables
    private bool onGround = false;
    private float distanceToGround = 0.5f;
    private float jumpForce = 7.6f;

    // Charavter variables
    private int movementSpeed = 6;
    private int rotationSpeed = 10;

    public GameObject mouseEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch (movementType)
        {
            case MovemntType.Keyboard:
                KeyboardMovement();
                break;
            case MovemntType.Mouse:
                MouseMovement();
                break;
            case MovemntType.Mobile:
                break;
        }

        #region Jump

        onGround = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

        if (Input.GetKeyDown(KeyCode.LeftShift) && onGround == true)
        {
            rb.velocity = Vector3.up * jumpForce;
        }

        #endregion

    }

    private void KeyboardMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        if (Horizontal != 0f || Vertical != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;
        Vector3 velocity = direction * movementSpeed;

        transform.Translate(velocity * Time.deltaTime);
    }

    private void MouseMovement()
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

        // Distance from player to taget position (mouse position)
        float distance = Vector3.Distance(transformPositionXZ, targetPositionXZ);

        if (distance >= 0.5f)
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

    // {F}
    public void RotateToMouse()
    {
        // Look At transform main character with his own axis (y) and target position (x, z)
        // In 2D (x, z) = (x, y)
        transform.LookAt(new Vector3(currentMousePosition.x, transform.position.y, currentMousePosition.z));

        // Set target position to player position to stop character
        targetPosition = transform.position;
    }

}

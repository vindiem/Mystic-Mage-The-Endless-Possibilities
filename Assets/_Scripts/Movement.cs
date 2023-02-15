using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public enum MovementType
    {
        Keyboard,
        Mouse,
        Mobile
    };
    public MovementType movementType;

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

    // Mobile
    [Header("Mobile movement")]
    public Joystick movementJoystick;
    public Joystick rotationJoystick;
    [SerializeField] private Button jumpButton;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        switch (movementType)
        {
            default:
                movementJoystick.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
                break;

            case MovementType.Mobile:
                movementJoystick.gameObject.SetActive(true);
                jumpButton.gameObject.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        switch (movementType)
        {
            case MovementType.Keyboard:
                KeyboardMovement();
                break;
            case MovementType.Mouse:
                MouseMovement();
                break;
            case MovementType.Mobile:
                MobileMovement();
                break;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Jump();
        }

        #region Get current mouse position

        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseWorldPosition, out RaycastHit raycastHit, CastLayer))
        {
            currentMousePosition = raycastHit.point;
        }

        #endregion

    }

    private void KeyboardMovement()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        #region Animations
        
        if (Horizontal != 0f || Vertical != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        #endregion

        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;
        RotateToMouse(currentMousePosition, false);
        transform.Translate(direction * movementSpeed * Time.deltaTime);

    }

    private void MouseMovement()
    {
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

    private void MobileMovement()
    {
        // Movement by movement joystick
        float Horizontal = movementJoystick.Horizontal;
        float Vertical = movementJoystick.Vertical;

        #region Animations

        if (Horizontal != 0f || Vertical != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        #endregion

        Vector3 direction = new Vector3(Horizontal, 0f, Vertical).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        // Rotation by rotation joystick
        float horizontalRotation = rotationJoystick.Horizontal;
        float verticalRotation = rotationJoystick.Vertical;

        if (horizontalRotation != 0 || verticalRotation != 0)
        {
            Vector3 rotateDirection = new Vector3(horizontalRotation, 0, verticalRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(rotateDirection), rotationSpeed * Time.deltaTime);
        }
    }

    // Jump [SHIFT]
    public void Jump()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

        if (onGround == true)
        {
            rb.velocity = Vector3.up * jumpForce;
        }
    }

    // {F}
    public void RotateToMouse(Vector3 mousePosition, bool S)
    {
        // Look At transform main character with his own axis (y) and target position (x, z)
        // In 2D (x, z) = (x, y)
        transform.LookAt(new Vector3(mousePosition.x, transform.position.y, mousePosition.z));

        // Set target position to player position to stop character
        if (S == true)
        {
            targetPosition = transform.position;
        }
    }

}

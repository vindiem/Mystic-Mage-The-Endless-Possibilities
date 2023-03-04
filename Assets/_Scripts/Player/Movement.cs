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
        Mobile,
        MobileV2
    };
    public MovementType movementType;

    public LayerMask CastLayer;
    private Animator animator;
    //private Rigidbody rb;

    // Vectors
    private Vector3 currentMousePosition = new Vector3(0, 0, 0);
    private Vector3 targetPosition = new Vector3(0, 0, 0);
    private Quaternion lookRotation;

    // Jump variables
    //private bool onGround = false;
    //private float distanceToGround = 0.5f;
    //private float jumpForce = 7.6f;

    // Charavter variables
    public float movementSpeed = 4;
    private int rotationSpeed = 10;

    public GameObject mouseEffect;

    // Mobile
    [Header("Mobile movement")]
    public Joystick movementJoystick;
    public Joystick movementJoystickV2;
    public Joystick attackJoystick;
    //[SerializeField] private Button jumpButton;
    //[SerializeField] private Scrollbar scrollbar;

    [Header("Sounds")]
    public AudioSource runningSound;
    public AudioSource breathingSound;

    private void Awake()
    {
        string mt = PlayerPrefs.GetString("MovementType");
        if (mt == "buttons")
        {
            movementType = MovementType.MobileV2;
        }
        else if (mt == "joystick")
        {
            movementType = MovementType.Mobile;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();

        switch (movementType)
        {
            default:
                movementJoystick.gameObject.SetActive(false);
                movementJoystickV2.gameObject.SetActive(false);
                attackJoystick.gameObject.SetActive(false);
                //jumpButton.gameObject.SetActive(false);
                //scrollbar.gameObject.SetActive(false);
                break;

            case MovementType.Mobile:
                movementJoystick.gameObject.SetActive(true);
                movementJoystickV2.gameObject.SetActive(false);
                attackJoystick.gameObject.SetActive(true);
                //jumpButton.gameObject.SetActive(true);
                //scrollbar.gameObject.SetActive(true);
                break;

            case MovementType.MobileV2:
                movementJoystick.gameObject.SetActive(false);
                movementJoystickV2.gameObject.SetActive(true);
                attackJoystick.gameObject.SetActive(false);
                break;
        }

        runningSound.mute = true;

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
                MobileMovement(movementJoystick);
                break;
            case MovementType.MobileV2:
                MobileMovement(movementJoystickV2);
                break;
        }

        /*if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Jump();
        }*/

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
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isRunning", true);
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isRunning", false);
        }

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

    private void MobileMovement(Joystick joystick)
    {
        // Get joystick axises
        float HorizontalAxis = joystick.Horizontal;
        float VerticalAxis = joystick.Vertical;

        if (HorizontalAxis != 0 || VerticalAxis != 0)
        {
            runningSound.mute = false;
            breathingSound.mute = true;

            Vector3 rotateDirection = new Vector3(HorizontalAxis, 0, VerticalAxis);

            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(rotateDirection), rotationSpeed * Time.deltaTime);

            // Move forward
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            animator.SetBool("isRunning", true);
        }
        else
        {
            runningSound.mute = true;
            breathingSound.mute = false;
            animator.SetBool("isRunning", false);
        }

    }

    // Jump [SHIFT]
    /*public void Jump()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

        if (onGround == true)
        {
            rb.velocity = Vector3.up * jumpForce;
        }
    }*/

    // Rotate to mouse position{F}
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

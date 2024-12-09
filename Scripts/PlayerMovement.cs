using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float slideSpeed = 15f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float slideDuration = 1f;
    public float slideFriction = 0.5f;

    [Header("Camera Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;

    private CharacterController controller;
    private Vector3 velocity;
    private float currentSpeed;
    private float originalHeight;
    private bool isCrouching = false;
    private bool isSliding = false;

    private float xRotation = 0f;
    public MapBoundary mapBoundary;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
        originalHeight = controller.height;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();

        if (mapBoundary != null)
        {
            // Apply damage if the player is outside the safe zone
            mapBoundary.ApplyDamageOutsideZone(transform);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        if (isSliding) return; // Disable regular movement during slide

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Handle crouching
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                controller.height = originalHeight / 2;
                currentSpeed = crouchSpeed;
                isCrouching = true;
            }
        }
        else
        {
            if (isCrouching)
            {
                controller.height = originalHeight;
                isCrouching = false;
            }
        }

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
        }
        else if (!isCrouching)
        {
            currentSpeed = walkSpeed;
        }

        // Check for sliding input
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(Slide(move));
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity and movement
        controller.Move(move * currentSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset vertical velocity if grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    IEnumerator Slide(Vector3 slideDirection)
    {
        isSliding = true;
        float slideTime = slideDuration;
        float currentSlideSpeed = slideSpeed;

        while (slideTime > 0)
        {
            controller.Move(slideDirection * currentSlideSpeed * Time.deltaTime);
            slideTime -= Time.deltaTime;
            currentSlideSpeed = Mathf.Lerp(slideSpeed, sprintSpeed, 1 - (slideTime / slideDuration)); // Gradual slow down
            yield return null;
        }

        isSliding = false;
    }
}

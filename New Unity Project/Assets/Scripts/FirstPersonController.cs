using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    PlayerInput playerInput;

    [Header("OnLook")]
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;
    public float rotationLimit = 90f;

    Vector2 lookInput;              // mouse
    float verticalRotation = 0f;    // camera
    float horizontalRotation = 0f;    // camera

    [SerializeField]
    GameObject standingHead;
    [SerializeField]
    GameObject crouchingHead;

    [Header("OnMove")]
    public float moveSpeed = 5f;
    Vector2 moveInput;              // WASD

    bool isCrouching;

    [SerializeField]
    Collider standingBody;
    [SerializeField]
    Collider crouchingBody;


    private void OnEnable()
    {
        // Looking with Mouse
        playerInput.actions.FindAction("Look").performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.actions.FindAction("Look").canceled += ctx => lookInput = Vector2.zero;
        playerInput.actions.FindAction("Look").Enable();

        // Moving with WASD
        playerInput.actions.FindAction("Move").performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions.FindAction("Move").canceled += ctx => moveInput = Vector2.zero;
        playerInput.actions.FindAction("Move").Enable();

        StandUp();
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("Look").Disable();
        playerInput.actions.FindAction("Move").Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (lookInput != Vector2.zero)
        {
            RotateCamera();
            RotateCharacter();
        }

        MoveCharacter();        
    }

    void RotateCamera()
    {
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -rotationLimit, rotationLimit);

        horizontalRotation += mouseX;

        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, Quaternion.Euler(verticalRotation, horizontalRotation, 0f), mouseSensitivity * Time.deltaTime);
    }

    void RotateCharacter()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
    }

    void MoveCharacter()
    {
        Vector3 forward = cameraTransform.forward;
        // this prevents the player from moving up or down based on the camera's tilt
        forward.y = 0f;

        Vector3 right = cameraTransform.right;
        // this prevents the player from moving up or down based on the camera's tilt
        right.y = 0f;


        Vector3 move = forward * moveInput.y + right * moveInput.x;

        if (move.magnitude > 1f)
            move.Normalize();

        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void OnCrouch()
    {
        // Start Crouch
        if (isCrouching)
        {
            StandUp();
        }
        else
        {
            CrouchDown();
        }
    }

    void CrouchDown()
    {
        isCrouching = true;

        standingBody.enabled = false;
        standingHead.SetActive(false);

        crouchingBody.enabled = true;
        crouchingHead.SetActive(true);

        cameraTransform.position = new Vector3(cameraTransform.position.x, crouchingHead.transform.position.y, cameraTransform.position.z);
    }

    void StandUp()
    {
        isCrouching = false;

        standingBody.enabled = true;
        standingHead.SetActive(true);

        crouchingBody.enabled = false;
        crouchingHead.SetActive(false);

        cameraTransform.position = new Vector3(cameraTransform.position.x, standingHead.transform.position.y, cameraTransform.position.z);
    }

}

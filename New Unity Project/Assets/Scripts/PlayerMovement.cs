using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float lookSpeed = 10f;

    bool isCrouching;

    [SerializeField]
    Collider standingBody;
    [SerializeField]
    Collider crouchingBody;

    [SerializeField]
    GameObject standingHead;
    [SerializeField]
    GameObject crouchingHead;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if(playerInput != null)
        {
            moveAction = playerInput.actions.FindAction("Move");
            lookAction = playerInput.actions.FindAction("Look");
        } else
        {
            Debug.LogWarning("Player Input not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        OnLook();
    }

    void OnMove()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector3 forwardMovement = transform.forward * direction.y * moveSpeed * Time.deltaTime;
        transform.Translate(forwardMovement, Space.World);
    }

    void OnLook()
    {
        float rotation = lookAction.ReadValue<float>();
        transform.Rotate(Vector3.up, rotation * lookSpeed * Time.deltaTime);
    }

    void OnCrouch()
    {
        // Start Crouch
        if(isCrouching)
        {
            StandUp();
        } else
        {
            CrouchDown();
        }

        Debug.Log("Is Crouching? " + isCrouching);
    }

    void CrouchDown()
    {
        isCrouching = true;
        standingBody.enabled = false;
        standingHead.SetActive(false);
        crouchingBody.enabled = true;
        crouchingHead.SetActive(true);
    }

    void StandUp()
    {
        isCrouching = false;
        standingBody.enabled = true;
        standingHead.SetActive(true);
        crouchingBody.enabled = false;
        crouchingHead.SetActive(false);
    }
}

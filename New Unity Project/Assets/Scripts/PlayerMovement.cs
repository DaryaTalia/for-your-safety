using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController playerController;
    PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction lookAction;
    [SerializeField]
    float moveSpeed = 5f;

    bool isCrouching;

    [SerializeField]
    Collider standingBody;
    [SerializeField]
    Collider crouchingBody;

    [SerializeField]
    GameObject standingHead;
    [SerializeField]
    GameObject crouchingHead;

    [SerializeField]
    Transform cameraTransform;

    private static PlayerMovement _instance = null;

    public static PlayerMovement Instance
    {
        get => _instance;
    }
    private void Awake()
    {
        if (_instance != null & _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions.FindAction("Move");
            lookAction = playerInput.actions.FindAction("Look");
        }
        else
        {
            Debug.LogWarning("Player Input not set");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    void OnMove()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3( movement.x, 0, movement.y);

        move = cameraTransform.forward * move.z  +  cameraTransform.right * move.x;

        playerController.Move(move * Time.deltaTime * moveSpeed);
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

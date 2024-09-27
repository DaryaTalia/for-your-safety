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
}

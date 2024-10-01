using UnityEngine;
using Unity.Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    float clampAngle = 80f;

    [SerializeField]
    float horizontalSpeed = 10f;
    [SerializeField]
    float verticalSpeed = 10f;

    [SerializeField]
    PlayerMovement playerMovement;
    Vector3 startingRotation;

    protected override void Awake()
    {
        playerMovement = PlayerMovement.Instance;
        base.Awake();
    }


    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if(startingRotation == null)
                {
                    startingRotation = transform.localRotation.eulerAngles;
                }

                Vector2 deltaInput = playerMovement.lookAction.ReadValue<Vector2>();

                startingRotation.x += deltaInput.x * Time.deltaTime * verticalSpeed;
                startingRotation.y += deltaInput.y * Time.deltaTime * horizontalSpeed;

                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }

}

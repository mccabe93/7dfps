using UnityEngine;
using UnityEngine.InputSystem;

// Reference:
// https://github.com/D-three/First-Person-Movement-Script-For-Unity/blob/main/First_Person_Movement.cs
public class PlayerMovement : MonoBehaviour
{
    private Vector3 MovementInput;
    private Vector3 MouseInput;
    private float RotationX = 0.0f;

    public Transform CameraTransform;

    public Rigidbody Rigidbody;

    public Transform PlayerTransform;

    private GroundedChecker _groundedChecker;

    public float Speed = 1.0f;

    public float JumpForce = 10.0f;

    public float SprintMultiplier = 2.0f;

    public float Sensitivity = 1.0f;

    public InputActionReference MoveAction;

    public InputActionReference MouseAction;

    public InputActionReference JumpAction;

    public InputActionReference SprintAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _groundedChecker = gameObject.AddComponent<GroundedChecker>();
    }

    void FixedUpdate()
    {
        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        if (_groundedChecker.IsGrounded && JumpAction.action.WasPressedThisDynamicUpdate())
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            return;
        }

        Vector2 input = MoveAction.action.ReadValue<Vector2>();
        float verticalSpeed = input.y * Speed;
        float horizontalSpeed = input.x * Speed;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0f, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        MovementInput.x = horizontalMovement.x;
        MovementInput.z = horizontalMovement.z;

        if (SprintAction.action.IsPressed())
        {
            MovementInput.x *= SprintMultiplier;
            MovementInput.z *= SprintMultiplier;
        }

        PlayerTransform.position = Vector3.MoveTowards(
            PlayerTransform.position,
            PlayerTransform.position + MovementInput,
            Speed
        );
    }

    private void MoveCamera()
    {
        MouseInput = MouseAction.action.ReadValue<Vector2>();
        RotationX -= MouseInput.y * Sensitivity;
        RotationX = Mathf.Clamp(RotationX, -90f, 90f);
        PlayerTransform.Rotate(0f, MouseInput.x * Sensitivity, 0f);
        CameraTransform.transform.localRotation = Quaternion.Euler(RotationX, 0f, 0f);
    }
}

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

    public float Speed = 3.0f;

    public float JumpForce = 300.0f;

    public float SprintTime = 0.5f;

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

    void Update()
    {
        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        if (_groundedChecker.IsGrounded && JumpAction.action.WasPressedThisFrame())
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            return;
        }

        Vector2 input = MoveAction.action.ReadValue<Vector2>();
        float verticalSpeed = input.y * Speed;
        float horizontalSpeed = input.x * Speed;

        if (horizontalSpeed == 0.0f && verticalSpeed == 0.0f)
        {
            SprintTime += Time.deltaTime;
            return;
        }

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0f, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        MovementInput.x = horizontalMovement.x;
        MovementInput.z = horizontalMovement.z;

        float speed = Speed;

        if (SprintAction.action.IsPressed() && SprintTime > 0)
        {
            speed *= SprintMultiplier;
            speed *= SprintMultiplier;
            SprintTime -= Time.deltaTime;
        }

        PlayerTransform.position = Vector3.MoveTowards(
            PlayerTransform.position,
            PlayerTransform.position + MovementInput,
            speed * Time.deltaTime
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

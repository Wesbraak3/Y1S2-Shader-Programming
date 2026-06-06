using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
    [Header("Character Input Values")]
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private Vector2 look;
    [SerializeField] private bool moveHigher = false;
    [SerializeField] private bool moveLower = false;
    [SerializeField] private bool sprint = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 15f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;

    private float yaw;
    private float pitch;

    private void OnEnable() {
    }
    private void Awake() {
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable() {
    }

    private void OnHorizontal(InputValue value) => moveDirection = value.Get<Vector2>();
    private void OnLook(InputValue value) => look = value.Get<Vector2>();
    private void OnHigher(InputValue value) => moveHigher = value.isPressed;
    private void OnLower(InputValue value) => moveLower = value.isPressed;
    private void OnSprint(InputValue value) => sprint = value.isPressed;

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleMovement() {
        float currentSpeed = sprint ? sprintSpeed : moveSpeed;

        Vector3 movement =
            transform.right * moveDirection.x +
            transform.forward * moveDirection.y;

        if (moveHigher)
            movement += Vector3.up;

        if (moveLower)
            movement += Vector3.down;

        transform.position += currentSpeed * Time.deltaTime * movement.normalized;
    }

    private void HandleLook() {
        yaw += look.x * mouseSensitivity;
        pitch -= Mathf.Clamp(look.y * mouseSensitivity, -80f, 80f);
       
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
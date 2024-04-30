using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    InputAction moveAction;
    InputAction cameraMoveAction;
    public float moveSpeed = 5f;
    public LayerMask groundMask;
    public Transform cameraTransform;
    public float cameraSpeed = 2f;
    public Transform cameraCapsule;
    private Rigidbody rb;
    private Vector2 moveInput;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        cameraMoveAction = playerInput.actions.FindAction("Look");
        moveAction.performed += contexto => moveInput = contexto.ReadValue<Vector2>();
        moveAction.canceled += contexto => moveInput = Vector2.zero;
        cameraMoveAction.performed += contexto => MoveCamera(contexto.ReadValue<Vector2>());
    }
    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }
    void MoveCamera(Vector2 direction)
    {
        cameraCapsule.Rotate(Vector3.up, direction.x * cameraSpeed * Time.deltaTime, Space.World);

        cameraTransform.Rotate(Vector3.right, -direction.y * cameraSpeed * Time.deltaTime, Space.Self);
    }
    void OnEnable()
    {
        moveAction.Enable();
        cameraMoveAction.Enable();
    }
    void OnDisable()
    {
        moveAction.Disable();
        cameraMoveAction.Disable();
    }
}

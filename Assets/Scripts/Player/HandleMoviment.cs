using Mirror;
using UnityEngine;

enum MovementState
{
    Walking,
    Running,
    Crouching,
    Idle
}

public class HandleMoviment : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private CharacterController m_characterController;

    [Header("Movement Settings")]
    [SerializeField] private float m_walkSpeed = 1.75f;
    [SerializeField] private float m_runSpeed = 3.5f;
    [SerializeField] private float m_crouchSpeed = 0.75f;
    [SerializeField] private float m_currentSpeed;

    [SerializeField] private MovementState m_movementState;

    Vector3 moveDirection;

    void Start()
    {
        m_handleInputs = GetComponent<HandleInputs>();
        m_characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; } // Ensure this script only runs for the local player

        moveDirection = m_handleInputs.MoveInput.y * m_cameraTransform.forward + 
                        m_handleInputs.MoveInput.x * m_cameraTransform.right;

        moveDirection.y = 0; // Keep the movement on the horizontal plane
        moveDirection.Normalize(); // Normalize the direction vector to ensure consistent speed

        handleStates(); // Handle movement states and speed

        m_characterController.Move(moveDirection * Time.deltaTime * m_currentSpeed); // Adjust speed as needed
        handleGravity(); // Handle gravity application
    }
    void handleStates()
    {
        if(m_handleInputs.Sprint && m_handleInputs.MoveInput != Vector3.zero) 
        {
            m_movementState = MovementState.Running;
            m_currentSpeed = m_runSpeed;
        }
        else if(m_handleInputs.Crouch) 
        {
            m_movementState = MovementState.Crouching;
            m_currentSpeed = m_crouchSpeed;
        }
        else if(m_handleInputs.MoveInput != Vector3.zero) 
        {
            m_movementState = MovementState.Walking;
            m_currentSpeed = m_walkSpeed;
        }
        else 
        {
            m_movementState = MovementState.Idle;
            m_currentSpeed = 0;
        }
    }
    void handleGravity()
    {
        if (!m_characterController.isGrounded)
        {
            float gravity = 9.81f; // Gravity value
            m_characterController.Move(Vector3.down * Time.deltaTime * gravity); // Apply gravity when grounded
        }
    }
}

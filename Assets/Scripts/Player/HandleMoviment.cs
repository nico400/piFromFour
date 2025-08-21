using Mirror;
using UnityEngine;

public class HandleMoviment : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private CharacterController m_characterController;


    [Header("Movement Settings")]
    [SerializeField] private float m_speed = 5f;

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

        m_characterController.Move(moveDirection * Time.deltaTime * m_speed); // Adjust speed as needed
        handleGravity(); // Handle gravity application
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

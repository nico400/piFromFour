using UnityEngine;

public class simpleStatePlayer : MonoBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;   

    enum MovementState
    {
        Walking,
        Running,
        Crouching,
        Idle
    }

    [SerializeField] private MovementState m_movementState;

    // Current speed of the player
    [SerializeField] private float m_walkSpeed = 1.75f;
    [SerializeField] private float m_runSpeed = 3.5f;
    [SerializeField] private float m_crouchSpeed = 0.75f;
    public float m_currentSpeed { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_handleInputs = GetComponent<HandleInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        handleStates();

    }
    public void handleStates()
    {
        if (m_handleInputs.Sprint && m_handleInputs.MoveInput != Vector3.zero)
        {
            m_movementState = MovementState.Running;
            m_currentSpeed = m_runSpeed;
        }
        else if (m_handleInputs.Crouch)
        {
            m_movementState = MovementState.Crouching;
            m_currentSpeed = m_crouchSpeed;
        }
        else if (m_handleInputs.MoveInput != Vector3.zero)
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
}

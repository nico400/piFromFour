using Mirror;
using System.Diagnostics;
using UnityEngine;

public class SimpleStatePlayer : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    private enum MovementState
    {
        Walking,
        Running,
        Crouching,
        Idle,
        Interacting
    }
    [SerializeField] private MovementState m_movementState;

    // Current speed of the player
    [SerializeField] private float m_walkSpeed = 1.75f;
    [SerializeField] private float m_runSpeed = 3.5f;
    [SerializeField] private float m_crouchSpeed = 0.75f;
    [SyncVar]        private bool m_iCanStandUp = true;
    [SyncVar]        private bool m_isInteracting = false;
    public float m_currentSpeed { get; private set; }
    public bool ICanStandUp
    {
        get => m_iCanStandUp;
        set => m_iCanStandUp = value;
    }
    public bool IsInteracting
    {
        get => m_isInteracting;
        set => m_isInteracting = value;
    }
    public void setInteractingValue(bool value) => m_isInteracting = value;

    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private LayerMask layerNotPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isLocalPlayer) { return; }
        m_characterController = GetComponent<CharacterController>();
        m_handleInputs = GetComponent<HandleInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        handleStatesSwitchs();
        switch (m_movementState)
        {
            case MovementState.Walking:
                // Handle walking logic here
                m_characterController.height = 2f;
                m_characterController.center = new Vector3(0, 1, 0);
                m_currentSpeed = m_walkSpeed;
                break;
            case MovementState.Running:
                // Handle running logic here
                m_characterController.height = 2f;
                m_characterController.center = new Vector3(0, 1, 0);
                m_currentSpeed = m_runSpeed;
                break;
            case MovementState.Crouching:
                // Handle crouching logic here
                m_characterController.height = 1.25f;
                m_characterController.center = new Vector3(0, 0.625f, 0);

                float radius = m_characterController.radius;
                float heightMax = 2;
                Vector3 p1 = transform.position + Vector3.up * ((heightMax / 2 ) + (radius / 2));
                Vector3 p2 = transform.position + Vector3.up * (heightMax - (radius / 2));

                Collider[] hitColliders = Physics.OverlapCapsule(p1, p2, radius, layerNotPlayer);

                if (hitColliders.Length > 0)
                {

                    m_iCanStandUp = false;
                }
                else
                {

                    m_iCanStandUp = true;
                }

                m_currentSpeed = m_crouchSpeed;
                break;
            case MovementState.Idle:
                // Handle idle logic here
                m_characterController.height = 2f;
                m_characterController.center = new Vector3(0, 1, 0);
                m_currentSpeed = 0;
                break;
            case MovementState.Interacting:
                // Handle idle logic here
                m_characterController.height = 2f;
                m_characterController.center = new Vector3(0, 1, 0);
                m_currentSpeed = 0;
                break;
        }
    }
    public void handleStatesSwitchs()
    {
        if (m_handleInputs.Sprint && m_handleInputs.MoveInput != Vector3.zero && m_iCanStandUp && !m_isInteracting)
            m_movementState = MovementState.Running;
        else if (m_handleInputs.Crouch && !m_isInteracting)
            m_movementState = MovementState.Crouching;
        else if (m_handleInputs.MoveInput != Vector3.zero && m_iCanStandUp && !m_isInteracting)
            m_movementState = MovementState.Walking;
        else if(m_isInteracting)
            m_movementState = MovementState.Interacting;
        else if (m_iCanStandUp && !m_isInteracting)
            m_movementState = MovementState.Idle;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float radius = m_characterController.radius;
        float heightMax = 2;
        Vector3 p1 = transform.position + Vector3.up * ((heightMax / 2) + (radius / 2));
        Vector3 p2 = transform.position + Vector3.up * (heightMax - (radius / 2));

        Gizmos.DrawWireSphere(p1, radius);
        Gizmos.DrawWireSphere(p2, radius);

    }
}

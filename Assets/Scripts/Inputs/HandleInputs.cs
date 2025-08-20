using UnityEngine;

public class HandleInputs : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions inputMappnig;

    private Vector2 m_moveInput;
    private Vector2 m_cameraInput;
    private bool m_Interaction;

    public Vector3 MoveInput
    {
        get => m_moveInput;
        set => m_moveInput = value;
    }
    public Vector2 CameraInput
    {
        get => m_cameraInput;
        set => m_cameraInput = value;
    }
    public bool Interaction
    {
        get => m_Interaction;
        set => m_Interaction = value;
    }

    private void Awake()
    {
        inputMappnig = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputMappnig.Enable();
    }
    private void OnDisable()
    {
        inputMappnig.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        m_moveInput = inputMappnig.Player.Move.ReadValue<Vector2>();
        m_cameraInput = inputMappnig.Player.Look.ReadValue<Vector2>();
        m_Interaction = inputMappnig.Player.Interact.WasPressedThisFrame();
    }
}

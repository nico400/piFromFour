using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleCamera : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform m_playerHead;

    [Header("config Camera")]
    [SerializeField] private float m_sensitivityX  = 25f;
    [SerializeField] private float m_sensitivityY = 25f;
    [SerializeField] private float minY = -60f;
    [SerializeField] private float maxY = 60f;

    float pitch;
    Vector2 m_cameraInput;

    Camera m_camera;

    [SyncVar] private float m_rotationX;
    void Start()
    {
        m_camera = gameObject.GetComponent<Camera>();
        if (!isLocalPlayer)
        {
            m_camera.enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        m_cameraInput = m_handleInputs.CameraInput;

        // Calcula rotação
        m_rotationX += m_cameraInput.x * m_sensitivityX * Time.deltaTime;

        pitch -= m_cameraInput.y * m_sensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Aplica rotação da câmera (X)
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Aplica rotação do corpo (Y)
        transform.parent.localRotation = Quaternion.Euler(0, m_rotationX, 0);

        // Segue a cabeça
        transform.position = m_playerHead.position;
    }
}

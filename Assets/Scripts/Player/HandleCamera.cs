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

    public float rotationX { get; private set; }

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
        m_cameraInput = m_handleInputs.CameraInput;

        rotationX += m_cameraInput.x * m_sensitivityX * Time.deltaTime;

        pitch -= m_cameraInput.y * m_sensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        transform.parent.localRotation = Quaternion.Euler(0, rotationX, 0);

        transform.position = m_playerHead.position;
    }
}

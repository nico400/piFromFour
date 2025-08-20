using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleCamera : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;

    [Header("config Camera")]
    [SerializeField] private float m_sencityvityX = 25f;
    [SerializeField] private float m_sencityvityY = 25f;
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

        rotationX += m_cameraInput.x * m_sencityvityX * Time.deltaTime;

        pitch += -m_cameraInput.y * m_sencityvityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        transform.rotation = Quaternion.Euler(pitch, rotationX, 0);
    }
}

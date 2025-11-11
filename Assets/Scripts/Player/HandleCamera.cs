using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleCamera : NetworkBehaviour
{
    public enum CameraMode
    {
        FreeLook,
        TargetLock,
        LockedCursorFree
    }

    [SerializeField] private CameraMode cameraMode = CameraMode.FreeLook;
    public CameraMode CurrentCameraMode
    {
        get => cameraMode;
        set => cameraMode = value;
    }

    [SerializeField] private HandleInputs handleInputs;
    [SerializeField] private Transform playerHead;

    [Header("Configuração da Câmera")]
    [SerializeField] private float sensitivityX = 25f;
    [SerializeField] private float sensitivityY = 25f;
    [SerializeField] private float minY = -60f;
    [SerializeField] private float maxY = 60f;

    private float pitch;
    private float yaw;
    private Vector2 cameraInput;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!isLocalPlayer && cam != null)
        {
            cam.enabled = false;
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        switch (cameraMode)
        {
            case CameraMode.FreeLook:
                HandleFreeLook();
                break;
            case CameraMode.TargetLock:
                SetCursorState(CursorLockMode.Locked, false);
                break;
            case CameraMode.LockedCursorFree:
                HandleLockedCursorFree();
                break;
        }
    }

    private void HandleFreeLook()
    {
        SetCursorState(CursorLockMode.Locked, false);

        cameraInput = handleInputs.CameraInput;

        yaw += cameraInput.x * sensitivityX * Time.deltaTime;
        pitch -= cameraInput.y * sensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Rotaciona a câmera no eixo X (pitch)
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Rotaciona o corpo do player no eixo Y (yaw)
        if (transform.parent != null)
            transform.parent.localRotation = Quaternion.Euler(0, yaw, 0);

        // Segue a cabeça do player
        if (playerHead != null)
            transform.position = playerHead.position;
    }
    private void HandleLockedCursorFree()
    {
        SetCursorState(CursorLockMode.Confined, true);

    }

    private void SetCursorState(CursorLockMode mode, bool visible)
    {
        Cursor.lockState = mode;
        Cursor.visible = visible;
    }

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
    }
}

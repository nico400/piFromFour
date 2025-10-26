using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class DoubleInteractable : NetworkBehaviour, IInteraction
{
    [SyncVar(hook = nameof(OnInteractableStateChanged))]
    [SerializeField] private bool m_isInteractableOn;

    [SerializeField] private DoubleInteractable m_otherInteractable;
    [SerializeField] private Transform m_playerTargetPosition;
    public Transform PlayerTargetPosition
    {
        get => m_playerTargetPosition;
        set => m_playerTargetPosition = value;
    }
    public UnityEvent isInteractivedStart;
    public UnityEvent isInteractivedFinish;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity whoCallInteractive)
    {
        if (m_otherInteractable == null) return;
        if (!m_isInteractableOn)
        {
            SetInteractable(true, whoCallInteractive);
            isInteractivedStart?.Invoke();
        }
    }
    [Command (requiresAuthority = false)]
    public void setInteractivedToFalse(NetworkIdentity player)
    {
        SetInteractable(false, player);
    }

    [ClientRpc]
    void SetInteractable(bool value, NetworkIdentity player)
    {
        m_isInteractableOn = value;

        var simpleState = player.GetComponent<SimpleStatePlayer>();
        if (simpleState != null)
            simpleState.setInteractingValue(value);

        var callObj = player.GetComponent<CallInteractivedObj>();
        if (callObj != null)
            callObj.SetCanInteraction(!value);

        var cam = player.GetComponentInChildren<HandleCamera>();
        if (cam != null)
        {
            if (value)
                cam.SetCameraMode(HandleCamera.CameraMode.TargetLock);
            else
                cam.SetCameraMode(HandleCamera.CameraMode.FreeLook);
        }
            
    }
    void OnInteractableStateChanged(bool oldValue, bool newValue)
    {
        if (m_otherInteractable == null) return;
        if (newValue && m_otherInteractable.m_isInteractableOn)
        {
            isInteractivedFinish?.Invoke();
        }
    }
}


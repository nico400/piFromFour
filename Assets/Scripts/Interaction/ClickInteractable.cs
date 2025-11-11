using Mirror;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class ClickInteractable : NetworkBehaviour, IInteraction
{
    [SyncVar (hook = nameof(InteractivedClick))]
    [SerializeField] private bool m_isInteractableOn = false;
    void setInteractableOn(bool value) => m_isInteractableOn = value;

    [SerializeField] private Transform m_playerCamTargetPosition;
    public Transform PlayerCamTargetPosition
    {
        get => m_playerCamTargetPosition;
        set => m_playerCamTargetPosition = value;
    }

    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        if (!m_isInteractableOn)
        {
            setInteractivedClick(true, WhoCallInteractive);
        }
    }

    [Command(requiresAuthority = false)]
    public void setInteractivedClick(bool value, NetworkIdentity player)
    {
        setInteractableOn(value);

        var simpleState = player.GetComponent<SimpleStatePlayer>();
        if (simpleState != null)
            simpleState.setInteractingValue(value);

        var callObj = player.GetComponent<CallInteractivedObj>();
        if (callObj != null)
            callObj.SetCanInteraction(!value);
    }

    void InteractivedClick(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            Debug.Log("Click Interactived!");
        }
    }
}

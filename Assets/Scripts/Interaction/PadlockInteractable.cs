using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class PadlockInteractable : NetworkBehaviour, IInteraction
{
    [SyncVar(hook = nameof(Interacived))]
    [SerializeField] private bool m_isInteractableOn;
    public bool IsInteractableOn
    {
        get => m_isInteractableOn;
        set => m_isInteractableOn = value;
    }
    public void setInteractableOn(bool value) => m_isInteractableOn = value;
    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        if (!m_isInteractableOn)
        {
            setInteractableOn(true);
        }
    }
    [ClientRpc]
    void Interacived(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            
        }
    }
}

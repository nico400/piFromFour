using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class SwitchInteractable : NetworkBehaviour, IInteraction
{
    public UnityEvent OnSwitchAcitived;
    public UnityEvent OnSwitchDeacitivated;

    bool isActive = false;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        RpcSwitchAcitived();
    }
    [ClientRpc]
    void RpcSwitchAcitived()
    {
        if (!isActive)
        {
            OnSwitchAcitived.Invoke();
            isActive = true;
        }
        else
        {
            OnSwitchDeacitivated.Invoke();
            isActive = false;
        }
    }
}

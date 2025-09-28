using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class ButtonInteractable : NetworkBehaviour, IInteraction
{
    public UnityEvent evntButtonActived;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity WhoCallInteractive)
    {       
        RpcButtonActived();
    }

    [ClientRpc]
    void RpcButtonActived()
    {
        evntButtonActived?.Invoke();
    }
}

using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class ButtonInteractable : NetworkBehaviour, IInteraction
{
    UnityEvent evntButtonActived;
    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        RpcButtonActived();
    }
    [ClientRpc]
    void RpcButtonActived()
    {
        evntButtonActived.Invoke();
    }
}

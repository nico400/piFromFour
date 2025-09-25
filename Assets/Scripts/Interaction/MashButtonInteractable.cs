using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class MashButtonInteractable : NetworkBehaviour, IInteraction
{
    [SyncVar]
    [SerializeField] private int m_currentMashCount = 0;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        m_currentMashCount++;
        RpcMashButtonActived();
    }
    [ClientRpc]
    void RpcMashButtonActived()
    {
        
    }
    private void Update()
    {
        if (m_currentMashCount >= 0)
            m_currentMashCount--;
    }
}

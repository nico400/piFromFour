using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class UseOnTargetInteractable : NetworkBehaviour, IInteraction
{
    [SerializeField] private string requiredTag = "Gas"; // opcional: tipo de objeto aceito
    [SerializeField] private UnityEvent OnUsedWith;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity caller)
    {
        var interactor = caller.GetComponent<CallInteractivedObj>();
        if (interactor != null)
        {
            var obj = interactor.ObjThatInteract;        
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(requiredTag) && !obj.CompareTag(requiredTag))
                    return;

                RpcUseWith();
            }
        }
    }

    [ClientRpc]
    void RpcUseWith()
    {
        print("aaaaa");
        OnUsedWith?.Invoke();
    }
}

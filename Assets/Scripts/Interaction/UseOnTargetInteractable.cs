using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class UseOnTargetInteractable : NetworkBehaviour, IInteraction
{
    [SerializeField] private string requiredTag = "Gas";
    [SerializeField] private UnityEvent OnUsedWith;
    [SyncVar] private bool m_used = false;

    public bool Used
    {
        get => m_used;
        set => m_used = value;
    }

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity caller)
    {
        var interactor = caller.GetComponent<CallInteractivedObj>();
        if (interactor != null && !m_used)
        {
            var obj = interactor.ObjThatInteract;
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(requiredTag) && !obj.CompareTag(requiredTag))
                    return;

                RpcUseWith(obj.netIdentity, caller);
            }
            m_used = true;
        }
    }

    [ClientRpc]
    void RpcUseWith(NetworkIdentity objNet, NetworkIdentity caller)
    {
        if (objNet == null)
        {
            Debug.LogWarning("NetworkIdentity do objeto é nula no Rpc!");
            return;
        }

        var obj = objNet.GetComponent<ObjectInteractable>();
        if (obj == null)
        {
            Debug.LogWarning("ObjectInteractable não encontrado!");
            return;
        }

        StartCoroutine(deleteGas(obj, caller));
        OnUsedWith?.Invoke();
    }

    IEnumerator deleteGas(ObjectInteractable obj, NetworkIdentity caller)
    {
        if (!isServer) yield break;

        if (obj == null)
        {
            Debug.LogWarning("Objeto null no deleteGas!");
            yield break;
        }

        if (caller == null)
        {
            Debug.LogWarning("Caller nulo no deleteGas!");
            yield break;
        }

        obj.DropObj(caller);
        yield return new WaitForSeconds(0.1f);

        if (NetworkServer.active)
            NetworkServer.Destroy(obj.gameObject);
    }
}

using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class ObjectInteractable : NetworkBehaviour, IInteraction
{
    public UnityEvent ActionWhenHold;
    public UnityEvent ActionWhenRelease;

    [SyncVar] private bool isHold = false; // sincronizado em todos

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity callerHand)
    {
        if (!isHold)
        {
            RpcPickupObj(callerHand.gameObject);
            isHold = true;
        }
    }

    [ClientRpc]
    void RpcPickupObj(GameObject hand)
    {
        var myHand = hand.GetComponent<CallInteractivedObj>().handThatInteractPosition;

        transform.SetParent(myHand.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        ActionWhenHold.Invoke();
    }
}

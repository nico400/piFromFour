using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity))]
public class ObjectInteractable : NetworkBehaviour, IInteraction
{
    private bool m_permissionThatHoldThisObject = true;

    public UnityEvent ActionWhenStartHold;
    public UnityEvent ActionWhenHold;
    public UnityEvent ActionWhenDrop;

    private NetworkIdentity m_networkIdentity;
    public NetworkIdentity GetNetIdentity
    {
        get => m_networkIdentity;
        set => m_networkIdentity = value;
    }

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity callerHand)
    {
        if (!m_permissionThatHoldThisObject) return;
        m_networkIdentity = callerHand;
        var hand = callerHand.GetComponent<CallInteractivedObj>();
        if (hand == null || hand.IsHold) return;

        m_permissionThatHoldThisObject = false;
        RpcPickupObj(callerHand.gameObject);
        hand.SetHoldParam(true);
    }

    [Command(requiresAuthority = false)]
    public void HoldObj(NetworkIdentity callerHand)
    {
        var hand = callerHand.GetComponent<CallInteractivedObj>();
        if (hand != null && hand.IsHold)
            RpcPickupHold(callerHand.gameObject);
    }

    [Command(requiresAuthority = false)]
    public void DropObj(NetworkIdentity callerHand)
    {
        var hand = callerHand.GetComponent<CallInteractivedObj>();
        if (hand != null && hand.IsHold)
        {
            m_permissionThatHoldThisObject = true;
            RpcPickupObjDrop(callerHand.gameObject);
            hand.SetHoldParam(false);
        }
    }

    [ClientRpc]
    void RpcPickupObj(GameObject hand)
    {
        var myHand = hand.GetComponent<CallInteractivedObj>().HandThatInteractPosition;
        var animationPickUp = hand.GetComponent<AnimationPickUpWeight>();
        animationPickUp.setPickUp(true);

        transform.SetParent(myHand.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        ActionWhenStartHold?.Invoke();
    }

    [ClientRpc]
    void RpcPickupHold(GameObject hand)
    {
        ActionWhenHold?.Invoke();
    }

    [ClientRpc]
    void RpcPickupObjDrop(GameObject hand)
    {
        var animationPickUp = hand.GetComponent<AnimationPickUpWeight>();
        animationPickUp.setPickUp(false);

        transform.SetParent(null);
        transform.localRotation = Quaternion.identity;
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        ActionWhenDrop?.Invoke();
    }
}

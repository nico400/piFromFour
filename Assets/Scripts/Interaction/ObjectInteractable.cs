using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Mirror.BouncyCastle.Asn1.Cmp.Challenge;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class ObjectInteractable : NetworkBehaviour, IInteraction
{
    public UnityEvent ActionWhenStartHold;
    public UnityEvent ActionWhenHold;
    public UnityEvent ActionWhenDrop;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity callerHand)
    {
        var hand = callerHand.GetComponent<CallInteractivedObj>();
        if (!hand.IsHold)
        {
            RpcPickupObj(callerHand.gameObject);
            hand.setHoldParam(true);
        }
    }
    [Command(requiresAuthority = false)]
    public void DropObj(NetworkIdentity callerHand)
    {
        var hand = callerHand.GetComponent<CallInteractivedObj>();
        if (hand.IsHold)
        {
            RpcPickupObjDrop(callerHand.gameObject);
            hand.setHoldParam(false);
        }
    }

    [ClientRpc]
    void RpcPickupObj(GameObject hand)
    {
        var myHand = hand.GetComponent<CallInteractivedObj>().handThatInteractPosition;
        var animationPickUp = hand.GetComponent<AnimationPickUpWeight>();
        animationPickUp.setPickUp(true);

        transform.SetParent(myHand.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        ActionWhenStartHold.Invoke();
    }
    [ClientRpc]
    void RpcPickupObjDrop(GameObject hand)
    {
        var animationPickUp = hand.GetComponent<AnimationPickUpWeight>();
        animationPickUp.setPickUp(false);

        transform.SetParent(null);
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        ActionWhenDrop.Invoke();
    }
}

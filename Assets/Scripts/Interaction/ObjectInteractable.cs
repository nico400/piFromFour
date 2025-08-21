using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class ObjectInteractable : NetworkBehaviour, IInteraction
{
    [Command(requiresAuthority = false)]
    public void interactived()
    {
        transform.position += Vector3.up * .15f; // Move the object up by 0.5 units 
    }
}

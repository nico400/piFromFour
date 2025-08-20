using Mirror;
using UnityEngine;

public class ObjectInteractable : NetworkBehaviour, IInteraction
{
    [Command(requiresAuthority = false)]
    public void interactived()
    {
        transform.position += Vector3.up * .1f; // Move the object up by 0.5 units 

        setPositionInAllClients(transform.position);
    }
    [ClientRpc]
    void setPositionInAllClients(Vector3 newPos)
    {
        transform.position = newPos; // Set the position of the object
    }
}

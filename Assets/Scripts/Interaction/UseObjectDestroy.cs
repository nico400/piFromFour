using Mirror;
using System.Collections;
using UnityEngine;

public class UseObjectDestroy : NetworkBehaviour
{
    public void DestroyObject(ObjectInteractable obj)
    {
        if (isServer)
            StartCoroutine(DestroyAfterDrop(obj));
    }

    IEnumerator DestroyAfterDrop(ObjectInteractable obj)
    {
        obj.DropObj(obj.GetNetIdentity);
        yield return new WaitForSeconds(0.1f);
        NetworkServer.Destroy(obj.gameObject);
    }
}

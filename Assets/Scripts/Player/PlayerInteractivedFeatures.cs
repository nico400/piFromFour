using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerInteractivedFeatures : NetworkBehaviour
{

    private bool isMovingToInteraction = false;
    private float moveSpeed = 3.5f;

    // Função pública chamada pelo script de interação
    public void MoveAndInteract(DoubleInteractable target, NetworkIdentity myIdentity)
    {
        if (!isLocalPlayer) return;
        if (isMovingToInteraction) return; // evita duplicar

        StartCoroutine(MoveToInteraction(target, myIdentity));
    }

    IEnumerator MoveToInteraction(DoubleInteractable target, NetworkIdentity myIdentity)
    {
        isMovingToInteraction = true;

        Transform destination = target.PlayerTargetPosition;
        // Mover suavemente até o ponto de interação
        while (Vector3.Distance(transform.position, destination.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Quando chega, chama o comando para o servidor processar
        target.interactived(myIdentity);

        isMovingToInteraction = false;
    }
    [TargetRpc]
    public void TargetChangePosition(NetworkConnection target, Vector3 newPos)
    {
        Debug.Log($"[Client:{isClient}] Teleporting to {newPos}");
        transform.position = newPos;
    }
}

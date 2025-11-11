using Mirror;
using System.Collections;
using UnityEngine;

public class PlayerInteractivedFeatures : NetworkBehaviour
{

    private bool isMovingToInteraction = false;
    private bool isCamMovingToInteraction = false;

    public void MoveCamAndInteractived(ClickInteractable target, Transform cam, NetworkIdentity player)
    {
        if (!isLocalPlayer) return;
        if (isCamMovingToInteraction) return;
        StartCoroutine(CamMoveToInteraction(target, cam, player));
    }

    IEnumerator CamMoveToInteraction(ClickInteractable target, Transform cam, NetworkIdentity player)
    {
        isCamMovingToInteraction = true;

        Transform destination = target.PlayerCamTargetPosition;
        while (Vector3.Distance(cam.transform.position, destination.position) > 0.1f)
        {
            float moveSpeed = 15f;
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isCamMovingToInteraction = false;
    }

    public void MoveAndInteract(DoubleInteractable target, NetworkIdentity myIdentity)
    {
        if (!isLocalPlayer) return;
        if (isMovingToInteraction) return;

        StartCoroutine(MoveToInteraction(target, myIdentity));
    }

    IEnumerator MoveToInteraction(DoubleInteractable target, NetworkIdentity myIdentity)
    {
        isMovingToInteraction = true;

        Transform destination = target.PlayerTargetPosition;
        while (Vector3.Distance(transform.position, destination.position) > 0.1f)
        {
            float moveSpeed = 15f;
            transform.position = Vector3.MoveTowards(transform.position, destination.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        isMovingToInteraction = false;
    }
    [TargetRpc]
    public void TargetChangePosition(NetworkConnection target, Vector3 newPos)
    {
        Debug.Log($"[Client:{isClient}] Teleporting to {newPos}");
        transform.position = newPos;
    }
}

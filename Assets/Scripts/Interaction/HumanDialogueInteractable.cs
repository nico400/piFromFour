using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class HumanDialogueInteractable : NetworkBehaviour, IInteraction
{
    [SerializeField] private string[] m_dialogue;
    [SerializeField] private string m_npcName;
    [SerializeField] private int m_indexDialogue = 0;

    [SerializeField] private Vector3 m_offsetInDistanceToDialogue;
    [SerializeField] private float m_distanceToDialogue = 2f;

    [SerializeField] private bool m_activeDialogue = false;

    NetworkIdentity m_networkIdentity;

    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        m_networkIdentity = WhoCallInteractive;
        if (!checkPlayerInDistanceToDialogue(m_networkIdentity)) return;

        if (!m_activeDialogue)
        {
            m_indexDialogue = 0;
            m_activeDialogue = true;
        }
        else
        {
            m_indexDialogue++;
        }
    }
    private void Update()
    {
        if (m_networkIdentity == null) return;
        if (!checkPlayerInDistanceToDialogue(m_networkIdentity))
            m_activeDialogue = false;

        if (m_activeDialogue)
        {
            m_networkIdentity.TryGetComponent<ActiveCanvasLocalPlayer>(out ActiveCanvasLocalPlayer activeCanvas);
            activeCanvas.ActiveDialoguePanel(true);
            activeCanvas.setNameNPC(m_npcName);
            if (m_indexDialogue < m_dialogue.Length)
            {
                activeCanvas.setDialogue(m_dialogue[m_indexDialogue]);
            }
            else
            {
                m_activeDialogue = false;
                activeCanvas.ActiveDialoguePanel(false);
            }
        }else
        {
            m_networkIdentity.TryGetComponent<ActiveCanvasLocalPlayer>(out ActiveCanvasLocalPlayer activeCanvas);
            activeCanvas.ActiveDialoguePanel(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + m_offsetInDistanceToDialogue, m_distanceToDialogue);
    }
    bool checkPlayerInDistanceToDialogue(NetworkIdentity player)
    {
        if(player == null) return false;

        Vector3 positionToCheck = transform.position + m_offsetInDistanceToDialogue;
        float distance = Vector3.Distance(player.transform.position, positionToCheck);

        if(distance <= m_distanceToDialogue)
            return true;
        else
            return false;
    }

}

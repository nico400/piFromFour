using Mirror;
using TMPro;
using UnityEngine;

public class ActiveCanvasLocalPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject m_canvasToActivate;


    [SerializeField] private GameObject m_painelDialogue;
    [SerializeField] private TextMeshProUGUI m_dialogueText;
    [SerializeField] private TextMeshProUGUI m_nameNPCText;

    public void ActiveDialoguePanel(bool isActive) => m_painelDialogue.SetActive(isActive);
    public void setDialogue(string dialogue) => m_dialogueText.text = dialogue;
    public void setNameNPC(string nameNPC) => m_nameNPCText.text = nameNPC;

    void Start()
    {
        if (isLocalPlayer)
        {
            m_canvasToActivate.SetActive(true);            
        }
        else
        {
            m_canvasToActivate.SetActive(false);
        }
        ActiveDialoguePanel(false);
    }
}

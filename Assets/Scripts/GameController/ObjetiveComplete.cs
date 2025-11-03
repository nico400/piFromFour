using Mirror;
using System.Linq;
using UnityEngine;

public class ObjetiveComplete : NetworkBehaviour
{
    [SerializeField] private UseOnTargetInteractable[] m_UsedTargets;
    void Update()
    {
        if (!isServer) return;

        bool allUsed = m_UsedTargets.All(i => i.Used);
        
    }
}

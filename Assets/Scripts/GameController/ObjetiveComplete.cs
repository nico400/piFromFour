using Mirror;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetiveComplete : NetworkBehaviour
{
    [SerializeField] private UseOnTargetInteractable[] m_UsedTargets;
    [SerializeField] private string nameScene;
    void Update()
    {
        if (!isServer) return;

        bool allUsed = m_UsedTargets.All(i => i.Used);
        if (allUsed)
        {
            RpcPlayObjetiveComplete();
        }
    }

    [ClientRpc]
    public void RpcPlayObjetiveComplete()
    {
        StartCoroutine(WaitAndLoadScene());     
    }
    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nameScene); 
    }
}

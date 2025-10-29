using UnityEngine;
using Mirror;

public class EnemyAlertEvent : NetworkBehaviour
{
    // Chamado pelo evento "ActionWhenDrop" do objeto
    public void OnObjectAction()
    {
        if (!isServer) return;

        StateManagerEnemy enemie = FindFirstObjectByType<StateManagerEnemy>();
        if (enemie == null) return;
        if (enemie.CurrentState == enemie.chase)
        {
            enemie.changeState(enemie.scare);
        }
    }
}

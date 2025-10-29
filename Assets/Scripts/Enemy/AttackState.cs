using Mirror;
using System.Collections;
using UnityEngine;

public class AttackState : NetworkBehaviour, States
{
    [SerializeField] private bool m_IsAttacink;
    public void enterState()
    {
        print("attacking");
        StartCoroutine(attacking());
    }

    public void exitState()
    {
        
    }

    public void updateState()
    {
        
    }
    
    IEnumerator attacking()
    {
        m_IsAttacink = true;
        yield return new WaitForSeconds(1f);
        StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.chase);
        m_IsAttacink = false;      
    }
}

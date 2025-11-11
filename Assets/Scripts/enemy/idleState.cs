using Mirror;
using UnityEngine;

public class IdleState : NetworkBehaviour, States
{
    /// <summary>
    /// enter in idle state, wait seconds and go to patrol state,
    /// but if player is in range go to chase state to chase the player
    /// </summary>

    [SerializeField] private float waitSeconds = .75f;
                     private float timer = 0f;
    public void enterState()
    {
        timer = 0f;

        SearchPlayer._onPlayerDetected += () => {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.chase);
        };
    }

    public void exitState()
    {
        
    }

    public void updateState()
    {   
        timer += Time.deltaTime;
        if (timer >= waitSeconds)
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
        }
    }
}

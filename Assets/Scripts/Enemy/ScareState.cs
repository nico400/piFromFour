using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ScareState : NetworkBehaviour, States
{
    [SerializeField] private SearchPlayer seachPlayer;
    [SerializeField] private NavMeshAgent agent;

    public void enterState()
    {
        print("Scare");
        agent.updateRotation = false; 
    }

    public void exitState()
    {
        agent.updateRotation = true;
    }

    public void updateState()
    {     

        if (seachPlayer.PlayerTransform != null)
        {
            Vector3 direction = (seachPlayer.PlayerTransform.position - transform.position).normalized;
            direction.y = 0;

            if (direction.magnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                float speed = 15f;
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            }

            if (seachPlayer.PlayerFound)
                agent.SetDestination(direction);
        }else
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
        }
    }
}

using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ScareState : NetworkBehaviour, States
{
    [SerializeField] private SeachPlayer seachPlayer;
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

        if (seachPlayer.playerTransform != null)
        {
            Vector3 direction = (seachPlayer.playerTransform.position - transform.position).normalized;
            direction.y = 0;

            if (direction.magnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                float speed = 15f;
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            }

            if (seachPlayer.playerFound)
                agent.SetDestination(direction);
        }else
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
        }
    }
}

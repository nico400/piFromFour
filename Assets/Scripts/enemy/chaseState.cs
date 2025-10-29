using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : NetworkBehaviour, States
{
    [SerializeField] private SeachPlayer seachPlayer;
    [SerializeField] private NavMeshAgent agent;
    public void enterState()
    {
        seachPlayer = GetComponent<SeachPlayer>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void exitState()
    {

    }

    public void updateState()
    {
        if (seachPlayer.playerFound)
            agent.SetDestination(seachPlayer.playerTransform.position);

        //muda de estado para outro

        float minDistance = 1.75f;
        if (agent.remainingDistance < minDistance)
        {
            // reached destination
            if (!seachPlayer.playerInView)
            {
                StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
            }
            /*else
            {
                agent.ResetPath();
                StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.attack);
            }*/
        }
    }
}

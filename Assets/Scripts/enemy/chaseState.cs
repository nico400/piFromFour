using UnityEngine;
using UnityEngine.AI;

public class chaseState : MonoBehaviour, States
{
    [SerializeField] private SeachPlayer seachPlayer;
    [SerializeField] private NavMeshAgent agent;
    public void enterState()
    {
        seachPlayer = GetComponent<SeachPlayer>();
        agent = GetComponent<NavMeshAgent>();
        print("Entering Chase State");
    }

    public void exitState()
    {
        
    }

    public void updateState()
    {
        if (seachPlayer.playerFound)
            agent.SetDestination(seachPlayer.playerTransform.position);

        float minDistance = 0.5f;
        if (agent.remainingDistance < minDistance)
        {
            if (!seachPlayer.playerInView)
            {
                StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
            }
        }
    }
}

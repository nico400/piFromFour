using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : NetworkBehaviour, States
{
    [SerializeField] PointList pointList;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float patrolSpeed = 3.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SeachPlayer._onPlayerDetected += () => {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.chase);
        };
    }

    public void enterState()
    {
        Debug.Log("Entering Patrol State");

        //choice random point from pointList
        if (pointList.points.Count == 0) return;

        if (!agent.hasPath)
        {
            int randomIndex = Random.Range(0, pointList.points.Count);
            agent.speed = patrolSpeed;
            agent.SetDestination(pointList.points[randomIndex]);
        }
    }

    public void exitState()
    {
        
    }

    public void updateState()
    {
        float distanceToPoint = 0.25f;
        if (agent.remainingDistance < distanceToPoint)
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.idle);
        }   
    }
}

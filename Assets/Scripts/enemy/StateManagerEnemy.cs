using Mirror;
using UnityEngine;

public class StateManagerEnemy : NetworkBehaviour
{
    public static StateManagerEnemy instance;

    States currentState;

    //states of the enemy
    public idleState idle = new idleState();
    public patrolState patrol = new patrolState();

    private void Awake()
    {
        instance = this;

        idle = GetComponent<idleState>();
        patrol = GetComponent<patrolState>();
    }

    private void Start()
    {
        currentState = idle;
        currentState.enterState();
    }

    void Update()
    {
        currentState.updateState();
    }

    public void changeState(States newState)
    {
        if (currentState != null)
        {
            currentState.exitState();
        }
        currentState = newState;
        currentState.enterState();
    }
}

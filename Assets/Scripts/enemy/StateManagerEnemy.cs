using Mirror;
using UnityEngine;

public class StateManagerEnemy : NetworkBehaviour
{
    public static StateManagerEnemy instance;

    States currentState;
    public States CurrentState
    { 
        get => currentState;
        set => currentState = value;
    }

    //states of the enemy
    [SyncVar] public IdleState idle = new IdleState();
    [SyncVar] public PatrolState patrol = new PatrolState();
    [SyncVar] public ChaseState chase = new ChaseState();
    [SyncVar] public AttackState attack = new AttackState();
    [SyncVar] public ScareState scare = new ScareState();

    private void Awake()
    {
        instance = this;

        idle = GetComponent<IdleState>();
        patrol = GetComponent<PatrolState>();
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

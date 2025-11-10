using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : NetworkBehaviour, States
{
    [SerializeField] private SearchPlayer seachPlayer;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float minDistance = 1.75f;

    public void enterState()
    {
        seachPlayer ??= GetComponent<SearchPlayer>();
        agent ??= GetComponent<NavMeshAgent>();

        if (agent == null)
            Debug.LogWarning($"{name} não tem NavMeshAgent!");
        if (seachPlayer == null)
            Debug.LogWarning($"{name} não tem SearchPlayer!");
    }

    public void exitState()
    {

    }

    public void updateState()
    {
        if (!isServer)
            return;

        if (seachPlayer == null || agent == null)
            return;

        // Se o inimigo perdeu o jogador, volta a patrulhar
        if (!seachPlayer.PlayerFound || seachPlayer.PlayerTransform == null)
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
            return;
        }

        // Persegue o jogador
        agent.SetDestination(seachPlayer.PlayerTransform.position);

        // Quando chega perto o suficiente
        if (!agent.pathPending && agent.remainingDistance <= minDistance)
        {
            var sst = seachPlayer.PlayerTransform.GetComponent<SimpleStatePlayer>();

            // 🔹 CORREÇÃO: Agora ataca somente se o jogador estiver visível E em pé
            if (seachPlayer.PlayerInView && sst != null && sst.ICanStandUp)
            {
                agent.ResetPath();
                StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.attack);
            }
            else
            {
                // Caso contrário, apenas patrulha (jogador se escondeu ou saiu da vista)
                StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
            }
        }
    }
}

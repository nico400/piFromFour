using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class HandleAnimations : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    private NetworkAnimator networkAnimator;

    void Awake()
    {
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    void Update()
    {
        if (!isServer)
            return; // 🔹 Apenas o servidor atualiza o Animator

        animator.SetFloat("Move", agent.velocity.magnitude);
    }

    [Server] // 🔹 Só o servidor pode iniciar a animação
    public void AttackAnimation()
    {
        animator.SetTrigger("attack");
        networkAnimator.SetTrigger("attack"); // garante sincronização
    }
}

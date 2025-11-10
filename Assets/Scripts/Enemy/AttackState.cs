using Mirror;
using System.Collections;
using UnityEngine;

public class AttackState : NetworkBehaviour, States
{
    [SerializeField] private bool m_IsAttacking;
    [SerializeField] private HandleAnimations animator;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackRange = 2f;

    private Transform target;

    public void enterState()
    {
        Debug.Log("Attack state entered");

        var search = GetComponent<SearchPlayer>();
        if (search != null)
            target = search.PlayerTransform;

        if (animator == null)
            animator = GetComponent<HandleAnimations>();

        m_IsAttacking = false;
    }

    public void exitState()
    {
        StopAllCoroutines();
        target = null;
        m_IsAttacking = false;
    }

    public void updateState()
    {
        if (!isServer)
            return;

        if (target == null)
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.patrol);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        // Se o jogador está longe, volta a perseguir
        if (distance > attackRange)
        {
            StateManagerEnemy.instance.changeState(StateManagerEnemy.instance.chase);
            return;
        }

        // Se pode atacar de novo, inicia o ataque
        if (!m_IsAttacking)
            StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        m_IsAttacking = true;

        // Ativa a animação de ataque
        if (animator != null)
            animator.AttackAnimation();

        // Aqui você pode checar colisão/dano ao jogador
        yield return new WaitForSeconds(attackCooldown);

        m_IsAttacking = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            print("aaaaa");
        }
    }
}

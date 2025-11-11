using Mirror;
using UnityEngine;

public class HandlerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private SimpleStatePlayer m_simpleStatePlayer;

    Vector3 moveInput;
    void Update()
    {
        if (!isLocalPlayer) { return; }
        if (m_simpleStatePlayer.IsInteracting) 
        {
            moveInput = Vector3.zero;

            animator.SetBool("crouch", false);
            animator.SetBool("run", false);

            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveZ", moveInput.y);

            return; 
        }

        moveInput = m_handleInputs.MoveInput;

        animator.SetBool("crouch", m_handleInputs.Crouch || !m_simpleStatePlayer.ICanStandUp);
        animator.SetBool("run", m_handleInputs.Sprint && moveInput.magnitude > 0);

        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveZ", moveInput.y);
    }
}

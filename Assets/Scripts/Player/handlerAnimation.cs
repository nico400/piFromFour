using Mirror;
using UnityEngine;

public class handlerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HandleInputs m_handleInputs;
    void Update()
    {
        if (!isLocalPlayer) { return; }

        Vector3 moveInput = m_handleInputs.MoveInput;
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveZ", moveInput.y);
    }
}

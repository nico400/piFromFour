using Mirror;
using UnityEngine;

public class ExampleDoorOpen : NetworkBehaviour
{
    [SerializeField] private Animator doorAnimator;

    public void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
    }
}

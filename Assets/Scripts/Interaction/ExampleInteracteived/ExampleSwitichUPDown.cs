using UnityEngine;

public class ExampleSwitichUPDown : MonoBehaviour
{
    [SerializeField] private Animator switchAnimator;

    public void ActiveSwitch()
    {
        if (switchAnimator != null)
        {
            switchAnimator.SetBool("IsActived", true);
        }
    }
    public void DeactiveSwitch()
    {
        if (switchAnimator != null)
        {
            switchAnimator.SetBool("IsActived", false);
        }
    }
}

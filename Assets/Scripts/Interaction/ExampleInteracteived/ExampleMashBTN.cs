using UnityEngine;

public class ExampleMashBTN : MonoBehaviour
{
    [SerializeField] private MashButtonInteractable mashButton;
    [SerializeField] private Animator animator;
    [SerializeField] private float valueMash;

    public void SetValueMash()
    {
        valueMash = mashButton.GetMashProgress();
        animator.SetFloat("Progress", valueMash);
    }

}

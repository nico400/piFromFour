using Mirror;
using UnityEngine;

public class AnimationPickUpWeight : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int layerIndex;
    [SerializeField] private float pickUpOn;
    [SerializeField] private float pickUpOff;
    [SerializeField] private bool isPickUp;
    public void setPickUp(bool value) => isPickUp = value;
    void Start()
    {
        if(!isLocalPlayer) { return; }
        animator.SetLayerWeight(layerIndex, pickUpOff);
        isPickUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }
        if(isPickUp)
            animator.SetLayerWeight(layerIndex, pickUpOn);
        else
            animator.SetLayerWeight(layerIndex, pickUpOff);
    }
}

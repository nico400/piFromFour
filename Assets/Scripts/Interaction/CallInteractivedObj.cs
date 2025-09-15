using Mirror;
using UnityEngine;

public class CallInteractivedObj : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform camTransform;
    [SerializeField] private LayerMask layerInteractables;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private GameObject m_handThatInteractPosition;
    [SerializeField] private ObjectInteractable objThatInteract;
    public GameObject handThatInteractPosition
    {
        get => m_handThatInteractPosition;
        private set => m_handThatInteractPosition = value;
    }

    [SyncVar] [SerializeField] private bool m_isHold = false;
    public bool IsHold 
    { 
        get => m_isHold; 
        set => m_isHold = value; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_handleInputs = GetComponent<HandleInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) { return; }

        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, interactionDistance, layerInteractables))
        {
            // Try to get the IInteraction component from the hit object
            IInteraction interactable = hit.collider.GetComponent<IInteraction>();
            if (interactable != null)
            {
                // Call the interactived method on the interactable object
                if (m_handleInputs.Interaction)
                {
                    objThatInteract = hit.collider.GetComponent<ObjectInteractable>();
                    interactable.interactived(this.netIdentity);
                }                  
            }
        }    
        if (m_isHold)
        {
            if (m_handleInputs.DropItem)
            {
                objThatInteract.DropObj(this.netIdentity);
            }
        }
    }
    public void setHoldParam(bool inHold) => m_isHold = inHold;
}
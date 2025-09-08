using Mirror;
using UnityEngine;

public class CallInteractivedObj : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform camTransform;
    [SerializeField] private LayerMask layerInteractables;
    [SerializeField] private float interactionDistance = 3f;

    [SerializeField] private GameObject m_handThatInteractPosition;
    public GameObject handThatInteractPosition
    {
        get => m_handThatInteractPosition;
        private set => m_handThatInteractPosition = value;
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

        Debug.DrawRay(camTransform.position, camTransform.forward * interactionDistance, Color.red); // Visualize the raycast in the scene view
        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, interactionDistance, layerInteractables))
        {
            // Try to get the IInteraction component from the hit object
            IInteraction interactable = hit.collider.GetComponent<IInteraction>();
            if (interactable != null)
            {
                // hand up when looking at an interactable object


                // Call the interactived method on the interactable object
                if (m_handleInputs.Interaction)
                    interactable.interactived(this.netIdentity);
            }
        }      
    }
}
using Mirror;
using UnityEngine;

public class CallInteractivedObj : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform camTransform;
    [SerializeField] private LayerMask layerInteractables;
    [SerializeField] private float interactionDistance = 3f;

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
        if (m_handleInputs.Interaction)
        {
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit , interactionDistance, layerInteractables))
            {
                IInteraction interactable = hit.collider.gameObject.GetComponent<IInteraction>();
                interactable.interactived();
            }
        }
    }
}
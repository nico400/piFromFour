using Mirror;
using UnityEngine;

public class CallInteractivedObj : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform m_camTransform;
    [SerializeField] private LayerMask m_layerInteractables;
    [SerializeField] private float m_interactionDistance = 1.5f;
    [SerializeField] private GameObject m_handThatInteractPosition;
    [SerializeField] private Transform m_cam;
    [SyncVar] private ObjectInteractable m_objThatInteract;
    [SyncVar] private bool m_isHold = false;
    [SyncVar] private DoubleInteractable m_doubleInteractable;
    [SyncVar] private bool m_canInteract = true;
    [SyncVar] private ClickInteractable m_clickInteractable;

    public GameObject HandThatInteractPosition => m_handThatInteractPosition;
    public bool IsHold
    {
        get => m_isHold;
        set => m_isHold = value;
    }
    public DoubleInteractable DoubleInteractableRef
    {
        get => m_doubleInteractable;
        set => m_doubleInteractable = value;
    }
    public bool CanInteract
    {
        get => m_canInteract;
        set => m_canInteract = value;
    }
    public ClickInteractable ClickInteractableRef
    {
        get => m_clickInteractable;
        set => m_clickInteractable = value;
    }

    void Start()
    {
        m_handleInputs = GetComponent<HandleInputs>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleRaycastInteraction();
        HandleObjectInteractions();
        HandleInteractableRelease();
    }

    private void HandleRaycastInteraction()
    {
        if (Physics.Raycast(m_camTransform.position, m_camTransform.forward, out RaycastHit hit, m_interactionDistance, m_layerInteractables))
        {
            IInteraction interactable = hit.collider.GetComponent<IInteraction>();
            if (interactable != null && !m_isHold && m_canInteract && m_handleInputs.Interaction)
            {
                m_objThatInteract = hit.collider.GetComponent<ObjectInteractable>();
                m_doubleInteractable = hit.collider.GetComponent<DoubleInteractable>();
                m_clickInteractable = hit.collider.GetComponent<ClickInteractable>();

                if (TryGetComponent<PlayerInteractivedFeatures>(out var playerFeatures))
                {
                    if (m_doubleInteractable != null)
                    {
                        playerFeatures.MoveAndInteract(m_doubleInteractable, netIdentity);
                    }
                }
                    

                interactable.interactived(netIdentity);
            }
        }
    }

    private void HandleObjectInteractions()
    {
        if (m_isHold && m_objThatInteract != null)
        {
            if (m_handleInputs.DropItem)
                m_objThatInteract.DropObj(netIdentity);

            if (m_handleInputs.Interaction)
                m_objThatInteract.HoldObj(netIdentity);
        }
    }

    private void HandleInteractableRelease()
    {
        if (m_doubleInteractable != null && m_handleInputs.DropItem && !m_canInteract)
        {
            m_doubleInteractable.setInteractivedToFalse(netIdentity);
            m_doubleInteractable = null;
        }
    }

    public void SetHoldParam(bool inHold) => m_isHold = inHold;
    public void SetCanInteraction(bool value) => m_canInteract = value;
}
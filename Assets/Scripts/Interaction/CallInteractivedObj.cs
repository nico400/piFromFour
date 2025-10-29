using Mirror;
using UnityEngine;

public class CallInteractivedObj : NetworkBehaviour
{
    [SerializeField] private HandleInputs m_handleInputs;
    [SerializeField] private Transform m_camTransform;
    [SerializeField] private LayerMask m_layerInteractables;
    [SerializeField] private float m_interactionDistance = 1.5f;
    [SerializeField] private GameObject m_handThatInteractPosition;

    [SyncVar] private NetworkIdentity m_objThatInteractNetId;
    [SyncVar] private bool m_isHold = false;
    [SyncVar] private bool m_canInteract = true;
    [SyncVar]
    [SerializeField] private ObjectInteractable m_objThatInteract;
    public GameObject HandThatInteractPosition => m_handThatInteractPosition;
    public bool IsHold { get => m_isHold; set => m_isHold = value; }
    public bool CanInteract { get => m_canInteract; set => m_canInteract = value; }
    public ObjectInteractable ObjThatInteract
    {
        get
        {
            if (m_objThatInteract == null && m_objThatInteractNetId != null)
                m_objThatInteract = m_objThatInteractNetId.GetComponent<ObjectInteractable>();
            return m_objThatInteract;
        }
        set
        {
            m_objThatInteract = value;
            m_objThatInteractNetId = value != null ? value.netIdentity : null;
        }
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
    }

    private void HandleRaycastInteraction()
    {
        if (!Physics.Raycast(m_camTransform.position, m_camTransform.forward, out RaycastHit hit, m_interactionDistance, m_layerInteractables))
            return;

        IInteraction interactable = hit.collider.GetComponent<IInteraction>();
        if (interactable == null) return;

        if (!m_isHold && m_canInteract && m_handleInputs.Interaction)
        {
            var interactableObj = hit.collider.GetComponent<ObjectInteractable>();
            if (interactableObj != null)
            {
                SetHeldObject(interactableObj);
                interactableObj.interactived(netIdentity);
            }
        }

        else if (m_isHold && ObjThatInteract != null && m_handleInputs.Interaction)
        {
            var useTarget = hit.collider.GetComponent<UseOnTargetInteractable>();
            if (useTarget != null)
            {
                useTarget.interactived(netIdentity);
            }
        }
    }

    public void SetHeldObject(ObjectInteractable obj)
    {
        ObjThatInteract = obj;
        CmdSetHeldObject(obj.netIdentity);
    }
    [Command]
    private void CmdSetHeldObject(NetworkIdentity objNetId)
    {
        if (objNetId != null)
            ObjThatInteract = objNetId.GetComponent<ObjectInteractable>();
        else
            ObjThatInteract = null;
    }

    private void HandleObjectInteractions()
    {
        if (m_isHold && ObjThatInteract != null)
        {
            if (m_handleInputs.DropItem)
                ObjThatInteract.DropObj(netIdentity);

            if (m_handleInputs.Interaction)
                ObjThatInteract.HoldObj(netIdentity);
        }
    }

    public void SetHoldParam(bool inHold) => m_isHold = inHold;
    public void SetCanInteraction(bool value) => m_canInteract = value;
}

using Mirror;
    using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class DoubleInteractable : NetworkBehaviour, IInteraction
{
    [SyncVar(hook = nameof(OnInteractableStateChanged))]
    [SerializeField] private bool isInteractableOn;

    [SerializeField] private DoubleInteractable otherInteractable;
    [SerializeField] private Transform m_playerTargetPosition;
    public Transform PlayerTargetPosition
    {
        get => m_playerTargetPosition;
        set => m_playerTargetPosition = value;
    }

    public UnityEvent isInteractivedStart;
    public UnityEvent isInteractivedFinish;

    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity whoCallInteractive)
    {
        if (otherInteractable == null) return;
        if (!isInteractableOn)
        {
            SetInteractable(true, whoCallInteractive);
            if (whoCallInteractive.TryGetComponent<PlayerInteractivedFeatures>(out var playerF))
            {
                // 🔥 Envia para o cliente se mover localmente
                playerF.TargetChangePosition(whoCallInteractive.connectionToClient, m_playerTargetPosition.position);
            }

            isInteractivedStart?.Invoke();
        }
    }
    [Command(requiresAuthority = false)]
    public void SetInteractable(bool value, NetworkIdentity player)
    {
        isInteractableOn = value;

        var simpleState = player.GetComponent<SimpleStatePlayer>();
        if (simpleState != null)
            simpleState.setInteractingValue(value);

        var callObj = player.GetComponent<CallInteractivedObj>();
        if (callObj != null)
            callObj.SetCanInteraction(!value);
    }
    void OnInteractableStateChanged(bool oldValue, bool newValue)
    {
        if (otherInteractable == null) return;
        if (newValue && otherInteractable.isInteractableOn)
        {
            isInteractivedFinish?.Invoke();
        }
    }
}


using Mirror;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkIdentity), typeof(NetworkTransformReliable))]
public class MashButtonInteractable : NetworkBehaviour, IInteraction
{
    [SerializeField] private int m_maxMashCount = 20;
    [SerializeField] private float m_decaySpeed = 5f;

    public UnityEvent onMashProgress;         // chamado enquanto está progredindo
    public UnityEvent onMashButtonCompleted;  // chamado quando completa

    [SyncVar(hook = nameof(OnProgressChanged))]
    private float m_currentMashProgress = 0f; // 0 → 1

    public float GetMashProgress() => m_currentMashProgress;

    private bool isCompleted = false;

    // chamado pelo jogador
    [Command(requiresAuthority = false)]
    public void interactived(NetworkIdentity WhoCallInteractive)
    {
        if (isCompleted) return;

        m_currentMashProgress += 2f / m_maxMashCount;
        m_currentMashProgress = Mathf.Clamp01(m_currentMashProgress);

        if (m_currentMashProgress >= 1f && !isCompleted)
        {
            isCompleted = true;
            RpcMashCompleted();
        }
    }

    private void Update()
    {
        if (!isServer || isCompleted) return;

        // Decaimento só no servidor
        if (m_currentMashProgress > 0f)
        {
            m_currentMashProgress -= (m_decaySpeed / m_maxMashCount) * Time.deltaTime;
            m_currentMashProgress = Mathf.Clamp01(m_currentMashProgress);
        }
    }

    // Hook chamado toda vez que o SyncVar muda
    private void OnProgressChanged(float oldValue, float newValue)
    {
        onMashProgress?.Invoke();
    }

    [ClientRpc]
    private void RpcMashCompleted()
    {
        onMashButtonCompleted?.Invoke();
        Debug.Log("MashButton completo!");
    }
}

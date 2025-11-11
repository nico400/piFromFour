using Mirror;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SearchPlayer : NetworkBehaviour
{
    [Header("Configurações de visão")]
    [SerializeField] private float rangeSphere = 5f;
    [Range(0f, 1f)][SerializeField] private float angleView = 0.5f;
    [SerializeField] private float heightView = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Perda de visão")]
    [SerializeField] private float timerToLosePlayer = 2f;
    private float time = 0f;

    [Header("Debug")]
    [SerializeField] private Vector3 lastPositionPlayerLose;

    public delegate void OnPlayerDetected();
    public static event OnPlayerDetected _onPlayerDetected;

    public Transform PlayerTransform { get; private set; }
    public GameObject PlayerOBJ { get; private set; }

    public bool PlayerFound { get; private set; }
    public bool PlayerInView { get; private set; }

    private void Update()
    {
        if (!isServer)
            return;

        HandleHiddenPlayer();
        HandleDetectionLogic();
    }

    /// <summary>
    /// Lida com o caso em que o jogador se esconde.
    /// </summary>
    private void HandleHiddenPlayer()
    {
        if (PlayerOBJ == null)
            return;

        if (PlayerOBJ.TryGetComponent(out SimpleStatePlayer statePlayer) && !statePlayer.ICanStandUp)
        {
            PlayerInView = false;

            if (PlayerFound)
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    ResetPlayerDetection();
                }
            }
        }
    }

    /// <summary>
    /// Lógica principal de busca e perda do jogador.
    /// </summary>
    private void HandleDetectionLogic()
    {
        // Se o player ainda está visível
        if (PlayerFound && PlayerTransform != null && IsPlayerVisible(PlayerTransform))
        {
            time = 0f;
            lastPositionPlayerLose = PlayerTransform.position;
            PlayerInView = true;
            return;
        }

        // Procura novos jogadores
        Vector3 pos = transform.position + Vector3.up * -0.25f;
        Collider[] colliders = Physics.OverlapSphere(pos, rangeSphere, playerLayer);

        Transform found = null;
        foreach (var col in colliders)
        {
            Transform t = col.transform;

            if (t.TryGetComponent(out SimpleStatePlayer ssp) && !ssp.ICanStandUp)
                continue;

            if (IsPlayerVisible(t))
            {
                found = t;
                break;
            }
        }

        if (found != null)
        {
            PlayerTransform = found;
            PlayerOBJ = found.gameObject;
            lastPositionPlayerLose = PlayerTransform.position;
            time = 0f;
            PlayerFound = true;
            PlayerInView = true;
            _onPlayerDetected?.Invoke();
        }
        else
        {
            PlayerInView = false;

            if (PlayerFound)
            {
                time += Time.deltaTime;

                if (PlayerTransform != null)
                    lastPositionPlayerLose = PlayerTransform.position;

                if (time >= timerToLosePlayer)
                {
                    ResetPlayerDetection();
                }
            }
        }
    }

    /// <summary>
    /// Verifica se o jogador está dentro do campo de visão e alcance.
    /// </summary>
    private bool IsPlayerVisible(Transform p)
    {
        if (p == null) return false;

        float distance = Vector3.Distance(transform.position, p.position);
        if (distance > rangeSphere) return false;

        float maxY = transform.position.y + heightView;
        float minY = transform.position.y - 0.25f;
        if (p.position.y < minY || p.position.y > maxY) return false;

        Vector3 dir = (p.position - transform.position).normalized;
        Vector3 localDir = transform.InverseTransformDirection(dir);

        if (localDir.z <= angleView)
            return false;

        // 🔹 Faz um raycast pra garantir que não há obstáculos
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out RaycastHit hit, rangeSphere))
        {
            if (!hit.transform.CompareTag("Player")) // supondo que seus jogadores têm a tag "Player"
                return false;
        }

        return true;
    }

    /// <summary>
    /// Reseta todos os dados relacionados à detecção.
    /// </summary>
    private void ResetPlayerDetection()
    {
        PlayerFound = false;
        PlayerInView = false;
        PlayerTransform = null;
        PlayerOBJ = null;
        time = 0f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up * -0.25f;
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(pos, Vector3.up, rangeSphere);
        Handles.DrawWireDisc(transform.position + Vector3.up * heightView, Vector3.up, rangeSphere);

        float p = angleView;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vectorLeft = (transform.forward * p + transform.right * -x) * rangeSphere;
        Vector3 vectorRight = (transform.forward * p + transform.right * x) * rangeSphere;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, vectorRight);
        Gizmos.DrawRay(transform.position, vectorLeft);
        Gizmos.DrawRay(transform.position + Vector3.up * heightView, vectorRight);
        Gizmos.DrawRay(transform.position + Vector3.up * heightView, vectorLeft);
        Gizmos.DrawRay(transform.position, Vector3.up * heightView);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastPositionPlayerLose, 0.25f);
    }
#endif
}

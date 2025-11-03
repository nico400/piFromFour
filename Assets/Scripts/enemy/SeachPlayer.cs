using UnityEditor;
using UnityEngine;

public class SeachPlayer : MonoBehaviour
{
    /// <summary>
    /// saerch for the player, when the player is in range, the enemy will follow the player,
    /// but if player is stealth, the enemy will not follow the player.
    /// 
    /// two ways to detect the player:
    /// in front of the enemy, and in range of the enemy, and player not stealth
    /// if makes a sound, the enemy will follow the player
    /// 
    /// </summary>
    /// 
     
    [SerializeField] private float rangeSphere;

    [Range(0,1)]
    [SerializeField] private float angleView;
    [SerializeField] private float heightView;
    [SerializeField] private LayerMask playerLayer;
    

    [SerializeField] private float timerToLosePlayer = 2f;
    [SerializeField] private Vector3 lastPositionPlayerLose;
    [SerializeField] private float time = 0;                     

    public delegate void OnPlayerDetected();
    public static event OnPlayerDetected _onPlayerDetected;

    public Transform playerTransform { get; private set; }
    [SerializeField] private GameObject playerObj;
    public GameObject PlayerOBJ
    {
        get => playerObj;
        set => playerObj = value;
    }
    public bool playerFound { get; private set; }
    public bool playerInView { get; private set; }

    private bool IsPlayerVisible(Transform p)
    {
        if (p == null) return false;

        float distance = Vector3.Distance(transform.position, p.position);
        if (distance > rangeSphere) return false;

        float maxY = transform.position.y + heightView;
        float minY = transform.position.y - 0.25f;
        if (!(p.position.y - 0.15f >= minY && p.position.y < maxY)) return false;

        Vector3 dir = (p.position - transform.position).normalized;
        Vector3 local = transform.InverseTransformVector(dir).normalized;
        if (local.z <= angleView) return false;

        return true;
    }

    public void Update()
    {

        if (playerFound && playerTransform != null && IsPlayerVisible(playerTransform))
        {
            time = 0f;
            lastPositionPlayerLose = playerTransform.position;
            playerInView = true;
            return; 
        }

        Vector3 pos = transform.position + Vector3.up * -0.25f;
        Collider[] colliders = Physics.OverlapSphere(pos, rangeSphere, playerLayer);

        Transform found = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform t = colliders[i].transform;
            if (IsPlayerVisible(t))
            {
                found = t;
                break;
            }
        }

        if (found != null)
        {
            // player detected

            playerTransform = found;
            playerObj = playerTransform.gameObject;
            lastPositionPlayerLose = playerTransform.position;
            time = 0f;
            playerFound = true;
            playerInView = true;
            _onPlayerDetected?.Invoke();
        }
        else
        {

            playerInView = false;
            playerObj = null;
            if (playerFound)
            {
                time += Time.deltaTime;
                if (playerTransform != null)
                    lastPositionPlayerLose = playerTransform.position;

                if (time >= timerToLosePlayer)
                {
                    playerFound = false;
                    playerTransform = null;
                    time = 0f;
                }
            }
        }

        if (playerObj != null)
        {
            // check if player is crouching and can't stand up (Stealfh)
            playerObj.TryGetComponent<SimpleStatePlayer>(out SimpleStatePlayer statePlayer);
            print(statePlayer.ICanStandUp);
            if (!statePlayer.ICanStandUp)
            {
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR

        Vector3 pos = transform.position + Vector3.up * -0.25f;
        Handles.DrawWireDisc(pos, Vector3.up, rangeSphere);
        Handles.DrawWireDisc(transform.position + Vector3.up * heightView, Vector3.up, rangeSphere);


        float p = angleView; // percent of the angleView (distance from vector foward)
        float x = Mathf.Sqrt(1 - p * p); // percent of the angleView

        Vector3 VecorLeft = (transform.forward * p + transform.right * -x) * rangeSphere; // left vector
        Vector3 VecorRight = (transform.forward * p + transform.right * x) * rangeSphere; // right vector

        Gizmos.color = Color.yellow;

        // draw view angle
        Gizmos.DrawRay(transform.position, VecorRight);
        Gizmos.DrawRay(transform.position, VecorLeft);
        Gizmos.DrawRay(transform.position + Vector3.up * heightView, VecorRight);
        Gizmos.DrawRay(transform.position + Vector3.up * heightView, VecorLeft);
        Gizmos.DrawRay(transform.position, Vector3.up * heightView);

        // draw last position player lose
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lastPositionPlayerLose, .55f);

        #endif
    }
}

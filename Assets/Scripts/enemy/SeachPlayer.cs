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
    public bool playerFound { get; private set; }
    public bool playerInView { get; private set; }

    public void Update()
    {
        // if player is in range of the enemy, and player not stealth, the enemy will follow the player
        //create sphere around the enemy to detect the player
        Collider[] colliders = Physics.OverlapSphere(transform.position, rangeSphere, playerLayer);
        if (colliders.Length > 0)
        {
            // check for each collider if is the player
            foreach (var collider in colliders)
            {
                // get the player transform
                collider.TryGetComponent(out Transform player);
                playerTransform = player;
                // get distance and direction to the player
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                Vector3 vecToWorld = (playerTransform.position - transform.position).normalized;
                Vector3 vecToTarget = transform.InverseTransformVector(vecToWorld).normalized;

                // check if the player is in range
                if (distance < rangeSphere)
                {
                    float maxValue = transform.position.y + heightView;
                    float minValue = transform.position.y;
                    // check if the player is in height view
                    if (playerTransform.position.y - 0.15f >= minValue || playerTransform.position.y < maxValue)
                    {
                        // check if the player is in front of the enemy
                        if (vecToTarget.z > angleView)
                        {
                            // if the player is not stealth, the enemy will follow the player
                            if (!playerFound)
                                _onPlayerDetected?.Invoke();

                            lastPositionPlayerLose = playerTransform.position;
                            time = 0;
                            playerFound = true;
                            playerInView = true;
                        }
                        else // if the player is not in front of the enemy
                        {
                            playerInView = false;
                        }
                    }
                }
            }
        }
        else if (playerInView && playerFound) // if the player is out of range, but was in view
        {
            playerInView = false;
        }

        if (playerFound && !playerInView) // if the player is out of range, but was found
        {
            // start timer to lose the player
            time += Time.deltaTime;
            lastPositionPlayerLose = playerTransform.position;
            if (time >= timerToLosePlayer) // if the timer is over, lose the player
            {
                playerFound = false;
                playerTransform = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position  + Vector3.up * - 0.15f, Vector3.up, rangeSphere);
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
    }
}

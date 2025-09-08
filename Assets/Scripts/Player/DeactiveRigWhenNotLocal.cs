using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DeactiveRigWhenNotLocal : NetworkBehaviour
{
    [SerializeField] private Rig rig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isLocalPlayer)
        {
            rig.weight = 0;
        }
    }
}

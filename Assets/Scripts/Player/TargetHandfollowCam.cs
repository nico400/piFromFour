using Mirror;
using UnityEngine;

public class TargetHandfollowCam : NetworkBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer) { return; }

        Vector3 newPos = cam.forward * offset.z + cam.up * offset.y + cam.right * offset.x + cam.position;
        target.position = newPos;
    }
}

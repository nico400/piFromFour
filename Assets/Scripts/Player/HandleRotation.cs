using Mirror;
using UnityEngine;

public class HandleRotation : NetworkBehaviour
{
    [SerializeField] private HandleCamera cameraOrientation;

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        float orientationY = cameraOrientation.rotationX;
        transform.rotation = Quaternion.Euler(0, orientationY, 0);
    }
}

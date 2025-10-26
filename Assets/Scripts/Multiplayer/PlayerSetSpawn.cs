using Mirror;
using UnityEngine;

public class PlayerSetSpawn : NetworkBehaviour
{
    [SyncVar] [SerializeField] private Transform spawnClientPlayer;
    void Start()
    {
        if (!isClient) return;


    }
}

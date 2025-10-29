using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPimenta : NetworkBehaviour
{
    [SerializeField] private List<Transform> SpawnPoint = new();
    [SerializeField] private GameObject PimentaPrefab;
    [SerializeField] private string tag;
    void Start()
    {
        if(isServer)
        {
            InvokeRepeating(nameof(SpawnInServer), 2f, 5f);
        }
    }
    [Server]
    void SpawnInServer()
    {
        int currentCount = GameObject.FindGameObjectsWithTag(tag).Length;
        int maxCount = 3;
        if (currentCount < maxCount)
        {
            Transform spawnPoint = SpawnPoint[Random.Range(0, SpawnPoint.Count)];
            GameObject obj = Instantiate(PimentaPrefab, spawnPoint.position, spawnPoint.rotation);
            NetworkServer.Spawn(obj);
        }
    }
}

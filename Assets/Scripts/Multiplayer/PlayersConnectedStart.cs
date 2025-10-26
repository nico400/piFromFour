using Mirror;
using System.Collections;
using UnityEngine;

public class PlayersConnectedStart : NetworkBehaviour
{

    [SerializeField] private PlayerConnected playerConnected;
    [SerializeField] private Animator animTransitionStart;

    [SyncVar(hook = nameof(OnPlayersConnectedChanged))]
    private int playersConnected;

    void Update()
    {
        if (!isServer) return;
        playersConnected = playerConnected.CheckPlayersConnected();
    }
    private void OnPlayersConnectedChanged(int oldValue, int newValue)
    {
        if (newValue >= 2)
        {
            StartCoroutine(WaitPlayerClientSpawn());
        }
    }
    IEnumerator WaitPlayerClientSpawn()
    {
        yield return new WaitForSeconds(1.75f);
        animTransitionStart.SetBool("IsClose", false);
    }
}


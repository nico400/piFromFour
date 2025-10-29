using Mirror;
using UnityEngine;

public class PlayerConnected : NetworkBehaviour
{
    public int CheckPlayersConnected()
    {
        int connectPlayers = NetworkServer.connections.Count;

        return connectPlayers;
    }
}

using UnityEngine;
using System.Collections;

public class StartSteamHost : MonoBehaviour
{
    [SerializeField] private SteamLobby steamLobby;
    IEnumerator Start()
    {
        while (steamLobby == null)
        {
            steamLobby = FindFirstObjectByType<SteamLobby>();
            yield return null;
        }

        yield return new WaitUntil(() => steamLobby != null && steamLobby.enabled);
        yield return new WaitForSeconds(0.5f); // dá tempo do SteamLobby inicializar
        print("Iniciando host Steam...");
        if (Mirror.NetworkClient.isConnected || Mirror.NetworkServer.active)
        {
            Debug.Log("Já existe uma conexão ativa, não é necessário iniciar o host.");
            yield break;
        }
        steamLobby.Host_lobby();
    }
}

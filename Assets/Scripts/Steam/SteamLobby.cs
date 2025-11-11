using Mirror;
using Steamworks;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string hostAddressKey = "HostAddress";
    [SerializeField] private NetworkManager m_networkManager;

    private Callback<LobbyCreated_t> m_LobbyCreated;
    private Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
    private Callback<LobbyEnter_t> m_LobbyEntered;

    private bool initialized = false;

    private IEnumerator Start()
    {
        // Aguarda até a Steam estar inicializada
        while (!SteamManager.Initialized)
        {
            if (text != null)
                text.text = "Waiting for Steam initialization...";
            yield return null;
        }
        while (m_networkManager == null)
        {
            m_networkManager = FindFirstObjectByType<NetworkManager>();
            yield return null;
        }

        initialized = true;
        if (text != null)
            text.text = "Steam initialized successfully.";

        // Registra os callbacks da Steam
        m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void Host_lobby()
    {
        if (!initialized)
        {
            Debug.LogWarning("Steam not initialized yet, cannot host lobby.");
            return;
        }

        if (m_networkManager == null)
        {
            Debug.LogError("NetworkManager reference is missing!");
            return;
        }

        Debug.Log("Creating Steam lobby...");
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, m_networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Lobby creation failed!");
            return;
        }

        Debug.Log("Lobby created successfully — starting host.");
        m_networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            hostAddressKey,
            SteamUser.GetSteamID().ToString()
        );
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Join request received — joining lobby...");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {

        if (SceneManager.GetActiveScene().name != "MainGame")
        {
            print("entrei na cena");
            SceneManager.LoadScene("MainGame");
        }


        Debug.Log("Joined Steam lobby.");

        if (NetworkServer.active)
            return;

        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            hostAddressKey
        );

        m_networkManager.networkAddress = hostAddress;
        m_networkManager.StartClient();
    }
}

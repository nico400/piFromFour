using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    public GameObject hostBTN = null;
    [SerializeField] private TextMeshProUGUI text;

    private NetworkManager m_networkManager;

    protected Callback<LobbyCreated_t> m_LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> m_LobbyEntered;

    private void Awake()
    {
        m_networkManager = GetComponent<NetworkManager>();
        if (!SteamManager.Initialized)
        {
            text.text = "Steam is not initialized.";
            return;
        }
        text.text = "Steam initialized.";

        m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        if (!SteamManager.Initialized) return;
        hostBTN.SetActive(false);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, m_networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Lobby creation failed");
            return;
        }
        Debug.Log("Lobby created successfully");
        m_networkManager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", SteamFriends.GetPersonaName() + "'s Lobby");
    }
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Join request received");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        Debug.Log("Joined lobby successfully");
        if (NetworkServer.active) return;
        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");
        m_networkManager.networkAddress = hostAddress;
        m_networkManager.StartClient();
    }
}

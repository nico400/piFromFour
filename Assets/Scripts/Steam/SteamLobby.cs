using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

// Classe responsável por gerenciar o lobby Steam integrado ao Mirror
public class SteamLobby : MonoBehaviour
{
    // Referência ao botão de host na interface
    [SerializeField] private GameObject hostBTN = null;
    // Texto para exibir status da Steam
    [SerializeField] private TextMeshProUGUI text;
    // Chave usada para salvar o endereço do host no lobby
    [SerializeField] string hostAdress = "HostAddress";

    // Referência ao NetworkManager do Mirror
    [SerializeField] private NetworkManager m_networkManager;

    // Callbacks para eventos do Steamworks
    protected Callback<LobbyCreated_t> m_LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> m_LobbyEntered;

    // Inicialização do lobby e registro dos callbacks
    private void Awake()
    {
        // Verifica se a Steam está inicializada
        if (!SteamManager.Initialized)
        {
            text.text = "Steam is not initialized.";
            return;
        }
        else
        {
            text.text = "Steam initialized.";
        }

        // Registra os callbacks para eventos de lobby
        m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // Método chamado ao clicar no botão de host para criar um lobby
    public void Host_lobby()
    {
        print("Creating Lobby");
        if (!SteamManager.Initialized) return;
       

        // Cria um lobby público na Steam
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, m_networkManager.maxConnections);
        // Desativa o botão de host após criar o lobby
        hostBTN.SetActive(false);
    }

    // Callback chamado quando o lobby é criado
    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        // Verifica se a criação do lobby foi bem-sucedida
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Lobby creation failed");
            return;
        }

        Debug.Log("Lobby created successfully");

        // Inicia o host do Mirror
        m_networkManager.StartHost();

        // Salva o SteamID do host no lobby para que outros jogadores possam conectar
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            hostAdress,
            SteamUser.GetSteamID().ToString()
        );
    }

    // Callback chamado quando há um pedido para entrar no lobby
    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Join request received");
        // Entra no lobby solicitado
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    // Callback chamado quando o jogador entra no lobby
    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        Debug.Log("Joined lobby successfully");

        // Se já for host, não faz nada
        if (NetworkServer.active) return;

        // Recupera o SteamID do host salvo no lobby
        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            hostAdress
        );

        // Define o endereço de rede do NetworkManager para conectar ao host
        // Com FizzySteamworks, isso é um SteamID, não um IP
        m_networkManager.networkAddress = hostAddress;

        // Inicia o cliente do Mirror para conectar ao host
        m_networkManager.StartClient();
    }
}

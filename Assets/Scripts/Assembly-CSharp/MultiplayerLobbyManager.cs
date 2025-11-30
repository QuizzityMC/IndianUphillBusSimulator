using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

/// <summary>
/// Manages the multiplayer lobby for the Indian Uphill Bus Simulator.
/// Allows players to host games, join existing games, and configure multiplayer settings.
/// </summary>
public class MultiplayerLobbyManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private InputField hostAddressInput;

    [SerializeField]
    private InputField portInput;

    [SerializeField]
    private InputField playerNameInput;

    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button joinButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private Text statusText;

    [SerializeField]
    private Text playerCountText;

    [SerializeField]
    private GameObject lobbyPanel;

    [SerializeField]
    private GameObject waitingPanel;

    [Header("Network Settings")]
    [SerializeField]
    private string defaultAddress = "localhost";

    [SerializeField]
    private int defaultPort = 7777;

    [SerializeField]
    private int maxPlayers = 8;

    private string playerName = "Player";
    private bool isHost = false;
    private int connectedPlayers = 0;

    private const string PLAYER_NAME_KEY = "PlayerName";
    private const string LAST_ADDRESS_KEY = "LastAddress";

    private void Start()
    {
        LoadSettings();
        SetupUI();
        ShowLobbyPanel();
    }

    private void LoadSettings()
    {
        playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player" + Random.Range(1000, 9999));
        string lastAddress = PlayerPrefs.GetString(LAST_ADDRESS_KEY, defaultAddress);

        if (hostAddressInput != null)
        {
            hostAddressInput.text = lastAddress;
        }

        if (portInput != null)
        {
            portInput.text = defaultPort.ToString();
        }

        if (playerNameInput != null)
        {
            playerNameInput.text = playerName;
        }
    }

    private void SaveSettings()
    {
        if (playerNameInput != null)
        {
            PlayerPrefs.SetString(PLAYER_NAME_KEY, playerNameInput.text);
        }

        if (hostAddressInput != null)
        {
            PlayerPrefs.SetString(LAST_ADDRESS_KEY, hostAddressInput.text);
        }

        PlayerPrefs.Save();
    }

    private void SetupUI()
    {
        if (hostButton != null)
        {
            hostButton.onClick.AddListener(OnHostButtonClicked);
        }

        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinButtonClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(OnStartGameClicked);
            startGameButton.gameObject.SetActive(false);
        }
    }

    private void ShowLobbyPanel()
    {
        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
        }

        if (waitingPanel != null)
        {
            waitingPanel.SetActive(false);
        }

        UpdateStatus("Ready to connect");
    }

    private void ShowWaitingPanel()
    {
        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(false);
        }

        if (waitingPanel != null)
        {
            waitingPanel.SetActive(true);
        }
    }

    public void OnHostButtonClicked()
    {
        SaveSettings();
        isHost = true;

        int port = GetPortFromInput();

        if (NetworkManager.singleton != null)
        {
            NetworkManager.singleton.NetworkPort = port;
            NetworkManager.singleton.MaxConnections = maxPlayers;
            NetworkManager.singleton.StartHost();
        }

        connectedPlayers = 1;
        UpdatePlayerCount();
        ShowWaitingPanel();
        UpdateStatus("Hosting game on port " + port);

        if (startGameButton != null)
        {
            startGameButton.gameObject.SetActive(true);
        }
    }

    public void OnJoinButtonClicked()
    {
        SaveSettings();
        isHost = false;

        string address = GetAddressFromInput();
        int port = GetPortFromInput();

        if (NetworkManager.singleton != null)
        {
            NetworkManager.singleton.NetworkAddress = address;
            NetworkManager.singleton.NetworkPort = port;
            NetworkManager.singleton.StartClient();
        }

        ShowWaitingPanel();
        UpdateStatus("Connecting to " + address + ":" + port);

        if (startGameButton != null)
        {
            startGameButton.gameObject.SetActive(false);
        }
    }

    public void OnBackButtonClicked()
    {
        if (NetworkManager.singleton != null)
        {
            if (isHost)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
        }

        SceneManager.LoadScene("HomeStart");
    }

    public void OnStartGameClicked()
    {
        if (isHost && NetworkManager.singleton != null)
        {
            NetworkManager.singleton.ServerChangeScene("Play(RCC)");
        }
    }

    private string GetAddressFromInput()
    {
        if (hostAddressInput != null && !string.IsNullOrEmpty(hostAddressInput.text))
        {
            return hostAddressInput.text;
        }
        return defaultAddress;
    }

    private int GetPortFromInput()
    {
        if (portInput != null && !string.IsNullOrEmpty(portInput.text))
        {
            int port;
            if (int.TryParse(portInput.text, out port))
            {
                return port;
            }
        }
        return defaultPort;
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
        Debug.Log("[Multiplayer] " + message);
    }

    private void UpdatePlayerCount()
    {
        if (playerCountText != null)
        {
            playerCountText.text = "Players: " + connectedPlayers + "/" + maxPlayers;
        }
    }

    public void OnPlayerConnected()
    {
        connectedPlayers++;
        UpdatePlayerCount();
        UpdateStatus("Player connected! Total: " + connectedPlayers);
    }

    public void OnPlayerDisconnected()
    {
        connectedPlayers = Mathf.Max(0, connectedPlayers - 1);
        UpdatePlayerCount();
        UpdateStatus("Player disconnected. Total: " + connectedPlayers);
    }

    public void OnConnectionFailed(string error)
    {
        UpdateStatus("Connection failed: " + error);
        ShowLobbyPanel();
    }

    public void OnDisconnected()
    {
        UpdateStatus("Disconnected from server");
        ShowLobbyPanel();
        connectedPlayers = 0;
        UpdatePlayerCount();
    }
}

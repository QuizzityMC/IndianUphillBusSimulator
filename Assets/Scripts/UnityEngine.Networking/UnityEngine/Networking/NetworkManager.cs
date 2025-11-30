using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager singleton { get; private set; }

        [Header("Network Settings")]
        [SerializeField]
        private string networkAddress = "localhost";

        [SerializeField]
        private int networkPort = 7777;

        [SerializeField]
        private int maxConnections = 8;

        [SerializeField]
        private string onlineScene = "Play(RCC)";

        [SerializeField]
        private string offlineScene = "HomeStart";

        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private List<GameObject> spawnablePrefabs = new List<GameObject>();

        [Header("State")]
        private bool isServer;
        private bool isClient;
        private bool isHost;

        public string NetworkAddress
        {
            get { return networkAddress; }
            set { networkAddress = value; }
        }

        public int NetworkPort
        {
            get { return networkPort; }
            set { networkPort = value; }
        }

        public int MaxConnections
        {
            get { return maxConnections; }
            set { maxConnections = value; }
        }

        public string OnlineScene
        {
            get { return onlineScene; }
            set { onlineScene = value; }
        }

        public string OfflineScene
        {
            get { return offlineScene; }
            set { offlineScene = value; }
        }

        public bool IsServer { get { return isServer; } }
        public bool IsClient { get { return isClient; } }
        public bool IsHost { get { return isHost; } }

        protected virtual void Awake()
        {
            if (singleton != null && singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (singleton == this)
            {
                singleton = null;
            }
        }

        public virtual void StartServer()
        {
            isServer = true;
            isClient = false;
            isHost = false;
            OnStartServer();
        }

        public virtual void StartClient()
        {
            isServer = false;
            isClient = true;
            isHost = false;
            OnStartClient();
        }

        public virtual void StartHost()
        {
            isServer = true;
            isClient = true;
            isHost = true;
            OnStartHost();
        }

        public virtual void StopServer()
        {
            isServer = false;
            OnStopServer();
        }

        public virtual void StopClient()
        {
            isClient = false;
            OnStopClient();
        }

        public virtual void StopHost()
        {
            StopServer();
            StopClient();
            isHost = false;
            OnStopHost();
        }

        protected virtual void OnStartServer()
        {
            Debug.Log("Server started on port " + networkPort);
            if (!string.IsNullOrEmpty(onlineScene))
            {
                SceneManager.LoadScene(onlineScene);
            }
        }

        protected virtual void OnStartClient()
        {
            Debug.Log("Client connecting to " + networkAddress + ":" + networkPort);
        }

        protected virtual void OnStartHost()
        {
            Debug.Log("Host started on port " + networkPort);
            OnStartServer();
        }

        protected virtual void OnStopServer()
        {
            Debug.Log("Server stopped");
            if (!string.IsNullOrEmpty(offlineScene))
            {
                SceneManager.LoadScene(offlineScene);
            }
        }

        protected virtual void OnStopClient()
        {
            Debug.Log("Client disconnected");
            if (!string.IsNullOrEmpty(offlineScene) && !isServer)
            {
                SceneManager.LoadScene(offlineScene);
            }
        }

        protected virtual void OnStopHost()
        {
            Debug.Log("Host stopped");
        }

        public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            if (playerPrefab != null)
            {
                GameObject player = Instantiate(playerPrefab);
                Debug.Log("Player added: " + playerControllerId);
            }
        }

        public virtual void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            Debug.Log("Player removed");
        }

        public virtual void OnServerConnect(NetworkConnection conn)
        {
            Debug.Log("Client connected: " + conn.connectionId);
        }

        public virtual void OnServerDisconnect(NetworkConnection conn)
        {
            Debug.Log("Client disconnected: " + conn.connectionId);
        }

        public virtual void OnClientConnect(NetworkConnection conn)
        {
            Debug.Log("Connected to server");
        }

        public virtual void OnClientDisconnect(NetworkConnection conn)
        {
            Debug.Log("Disconnected from server");
        }

        public virtual void OnClientError(NetworkConnection conn, int errorCode)
        {
            Debug.LogError("Client error: " + errorCode);
        }

        public void ServerChangeScene(string newSceneName)
        {
            if (isServer)
            {
                SceneManager.LoadScene(newSceneName);
            }
        }
    }

    // Placeholder for NetworkConnection
    public class NetworkConnection
    {
        public int connectionId;
        public string address;

        public virtual bool Send(short msgType, MessageBase msg)
        {
            return true;
        }
    }

    // Placeholder for PlayerController
    public class PlayerController
    {
        public short playerControllerId;
        public GameObject gameObject;
        public bool IsValid { get { return gameObject != null; } }
    }

    // Placeholder for MessageBase
    public abstract class MessageBase
    {
        public virtual void Deserialize(NetworkReader reader) { }
        public virtual void Serialize(NetworkWriter writer) { }
    }

    // Placeholder for NetworkReader
    public class NetworkReader
    {
        public byte[] buffer;
        public int Position { get; private set; }
    }

    // Placeholder for NetworkWriter
    public class NetworkWriter
    {
        public byte[] buffer = new byte[1024];
        public int Position { get; private set; }
    }
}
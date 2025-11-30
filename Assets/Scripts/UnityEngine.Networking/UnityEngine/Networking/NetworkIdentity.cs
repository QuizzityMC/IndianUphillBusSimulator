using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
    public class NetworkIdentity : MonoBehaviour
    {
        [SerializeField]
        private uint m_SceneId;

        [SerializeField]
        private bool m_ServerOnly;

        [SerializeField]
        private bool m_LocalPlayerAuthority;

        private uint m_NetId;
        private bool m_IsServer;
        private bool m_IsClient;
        private bool m_IsLocalPlayer;
        private bool m_HasAuthority;
        private short m_PlayerControllerId = -1;

        private NetworkConnection m_ConnectionToServer;
        private NetworkConnection m_ConnectionToClient;

        private List<NetworkBehaviour> m_NetworkBehaviours;

        public uint netId { get { return m_NetId; } }
        public uint sceneId { get { return m_SceneId; } }
        public bool serverOnly { get { return m_ServerOnly; } set { m_ServerOnly = value; } }
        public bool localPlayerAuthority { get { return m_LocalPlayerAuthority; } set { m_LocalPlayerAuthority = value; } }
        public bool isServer { get { return m_IsServer; } }
        public bool isClient { get { return m_IsClient; } }
        public bool isLocalPlayer { get { return m_IsLocalPlayer; } }
        public bool hasAuthority { get { return m_HasAuthority; } }

        public NetworkConnection connectionToServer { get { return m_ConnectionToServer; } }
        public NetworkConnection connectionToClient { get { return m_ConnectionToClient; } }
        public short playerControllerId { get { return m_PlayerControllerId; } }

        public static uint GetNextNetworkId()
        {
            return s_NextNetworkId++;
        }

        private static uint s_NextNetworkId = 1;

        private void Awake()
        {
            m_NetworkBehaviours = new List<NetworkBehaviour>(GetComponentsInChildren<NetworkBehaviour>());
        }

        internal void SetNetworkId(uint newNetId)
        {
            m_NetId = newNetId;
        }

        internal void SetIsServer(bool value)
        {
            m_IsServer = value;
        }

        internal void SetIsClient(bool value)
        {
            m_IsClient = value;
        }

        internal void SetLocalPlayer(short controllerId)
        {
            m_IsLocalPlayer = true;
            m_PlayerControllerId = controllerId;
            m_HasAuthority = true;
        }

        internal void SetConnectionToServer(NetworkConnection conn)
        {
            m_ConnectionToServer = conn;
        }

        internal void SetConnectionToClient(NetworkConnection conn)
        {
            m_ConnectionToClient = conn;
        }

        internal void SetAuthority(bool authority)
        {
            m_HasAuthority = authority;
        }

        public void OnStartServer()
        {
            m_IsServer = true;
            if (m_NetId == 0)
            {
                m_NetId = GetNextNetworkId();
            }

            foreach (var behaviour in m_NetworkBehaviours)
            {
                behaviour.OnStartServer();
            }
        }

        public void OnStartClient()
        {
            m_IsClient = true;

            foreach (var behaviour in m_NetworkBehaviours)
            {
                behaviour.OnStartClient();
            }
        }

        public void OnStartLocalPlayer()
        {
            foreach (var behaviour in m_NetworkBehaviours)
            {
                behaviour.OnStartLocalPlayer();
            }
        }

        public void OnStartAuthority()
        {
            m_HasAuthority = true;
            foreach (var behaviour in m_NetworkBehaviours)
            {
                behaviour.OnStartAuthority();
            }
        }

        public void OnStopAuthority()
        {
            m_HasAuthority = false;
            foreach (var behaviour in m_NetworkBehaviours)
            {
                behaviour.OnStopAuthority();
            }
        }

        public void OnNetworkDestroy()
        {
            foreach (var behaviour in m_NetworkBehaviours)
            {
                behaviour.OnNetworkDestroy();
            }
        }

        public void RebuildObservers(bool initialize)
        {
            // Rebuild observers
        }

        public void AssignClientAuthority(NetworkConnection conn)
        {
            m_ConnectionToClient = conn;
            m_HasAuthority = false;
        }

        public void RemoveClientAuthority(NetworkConnection conn)
        {
            m_ConnectionToClient = null;
        }
    }
}
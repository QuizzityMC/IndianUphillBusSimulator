using UnityEngine;
using System;

namespace UnityEngine.Networking
{
    public class NetworkBehaviour : MonoBehaviour
    {
        private NetworkIdentity m_NetworkIdentity;

        public NetworkIdentity netIdentity
        {
            get
            {
                if (m_NetworkIdentity == null)
                {
                    m_NetworkIdentity = GetComponent<NetworkIdentity>();
                }
                return m_NetworkIdentity;
            }
        }

        public bool isServer
        {
            get { return netIdentity != null && netIdentity.isServer; }
        }

        public bool isClient
        {
            get { return netIdentity != null && netIdentity.isClient; }
        }

        public bool isLocalPlayer
        {
            get { return netIdentity != null && netIdentity.isLocalPlayer; }
        }

        public bool hasAuthority
        {
            get { return netIdentity != null && netIdentity.hasAuthority; }
        }

        public uint netId
        {
            get { return netIdentity != null ? netIdentity.netId : 0; }
        }

        public NetworkConnection connectionToServer
        {
            get { return netIdentity != null ? netIdentity.connectionToServer : null; }
        }

        public NetworkConnection connectionToClient
        {
            get { return netIdentity != null ? netIdentity.connectionToClient : null; }
        }

        public short playerControllerId
        {
            get { return netIdentity != null ? netIdentity.playerControllerId : (short)-1; }
        }

        protected virtual void OnEnable()
        {
            m_NetworkIdentity = GetComponent<NetworkIdentity>();
        }

        public virtual void OnStartServer() { }
        public virtual void OnStartClient() { }
        public virtual void OnStartLocalPlayer() { }
        public virtual void OnStartAuthority() { }
        public virtual void OnStopAuthority() { }
        public virtual void OnNetworkDestroy() { }

        public virtual bool OnSerialize(NetworkWriter writer, bool initialState)
        {
            return false;
        }

        public virtual void OnDeserialize(NetworkReader reader, bool initialState) { }

        public virtual void OnSetLocalVisibility(bool vis) { }
        public virtual void OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize) { }
        public virtual bool OnCheckObserver(NetworkConnection conn) { return true; }

        protected void SetDirtyBit(uint dirtyBit)
        {
            // Mark sync var as dirty
        }

        protected void ClearAllDirtyBits()
        {
            // Clear all dirty bits
        }

        public void SetSyncVar<T>(T value, ref T fieldValue, uint dirtyBit)
        {
            if (!EqualityComparer<T>.Default.Equals(value, fieldValue))
            {
                fieldValue = value;
                SetDirtyBit(dirtyBit);
            }
        }
    }

    // HashSet placeholder for backwards compatibility
    public class HashSet<T> : System.Collections.Generic.HashSet<T>
    {
        public HashSet() : base() { }
        public HashSet(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }
    }

    // Generic EqualityComparer
    public class EqualityComparer<T>
    {
        private static EqualityComparer<T> _default;

        public static EqualityComparer<T> Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new EqualityComparer<T>();
                }
                return _default;
            }
        }

        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }
    }
}
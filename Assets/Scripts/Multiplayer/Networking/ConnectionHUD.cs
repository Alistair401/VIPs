using UnityEngine;
using UnityEngine.Networking;

namespace Multiplayer.Networking
{
    public class ConnectionHUD : MonoBehaviour
    {
        public NetworkManager NetworkManager;

        private void Awake()
        {
            NetworkManager = GetComponent<NetworkManager>();
        }

        private void Start()
        {
            Debug.Log("Press S for server, H for host, C for client");
        }

        private void Update()
        {
            if (!NetworkManager.IsClientConnected() && !NetworkServer.active)
            {
                if (Input.GetKeyDown(KeyCode.S))
                    NetworkManager.StartServer();
                if (Input.GetKeyDown(KeyCode.H))
                    NetworkManager.StartHost();
                if (Input.GetKeyDown(KeyCode.C))
                    NetworkManager.StartClient();
            }
        }
    }
}
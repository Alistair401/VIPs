using Camera;
using UnityEngine;
using UnityEngine.Networking;

namespace Player
{
    public class PlayerManager : NetworkBehaviour
    {
        public float Speed;
        public UnityEngine.Camera Camera;

        public PlayerInventory PlayerInventory { get; set; }
        public PlayerTransactions PlayerTransactions { get; set; }
        public PlayerMovement PlayerMovement { get; set; }
        private PlayerCollisions _playerCollisions;
        private PlayerActions _playerActions;

        private int _health;

        private void Start()
        {
            Camera = UnityEngine.Camera.main;
            Camera.GetComponent<CameraManager>().Subject = gameObject;
            PlayerMovement = gameObject.AddComponent<PlayerMovement>();
            PlayerTransactions = gameObject.AddComponent<PlayerTransactions>();
            _playerCollisions = gameObject.AddComponent<PlayerCollisions>();
            _playerActions = gameObject.AddComponent<PlayerActions>();
            PlayerInventory = gameObject.AddComponent<PlayerInventory>();
        }
    }
}
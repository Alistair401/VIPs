using System;
using Pickups;
using Pickups.Weapons;
using UnityEngine;

namespace Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        private PlayerManager _playerManager;

        private void Start()
        {
            _playerManager = gameObject.GetComponent<PlayerManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Pickup>() != null)
            {
                other.gameObject.GetComponent<Pickup>().Activate(_playerManager.PlayerInventory);
            }
        }
    }
}
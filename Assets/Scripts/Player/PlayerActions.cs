using Items;
using Pickups;
using Projectiles;
using UnityEngine;

namespace Player
{
    public class PlayerActions : MonoBehaviour
    {
        private PlayerManager _playerManager;

        private void Start()
        {
            _playerManager = gameObject.GetComponent<PlayerManager>();
        }

        private void Update()
        {
            if (Input.GetAxis("Fire1") > 0.5)
            {
                Item heldItem = _playerManager.PlayerInventory.GetHeldItem();
                if (heldItem != null)
                {
                    heldItem.Fire(this);
                }
            }
        }

        public void Fire(GameObject firePrefab, float range, int damage)
        {
            Projectile projectile =
                Instantiate(firePrefab, gameObject.transform.position, gameObject.transform.rotation)
                    .GetComponent<Projectile>();
            projectile.SetRange(range);
            projectile.Damage = damage;
        }
    }
}
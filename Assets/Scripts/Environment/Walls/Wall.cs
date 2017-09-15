using Helpers;
using Projectiles;
using UnityEngine;

namespace Environment.Walls
{
    public class Wall: MonoBehaviour, IRemoteTrigger
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Destroy();
            }
        }

        public void Trigger(Collider2D triggeringCollider)
        {
            OnTriggerEnter2D(triggeringCollider);
        }
    }
}
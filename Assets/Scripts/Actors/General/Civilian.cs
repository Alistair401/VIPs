using Pickups;
using Projectiles;
using UnityEngine;

namespace Actors.General
{
    public class Civilian: Actor
    {
        public void OnTriggerEnter2D(Collider2D other)
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                DamageSelf(projectile.Damage);
                projectile.Destroy();

            }
        }

        private void DamageSelf(int amount)
        {
        }

        public override void Trigger(Collider2D triggeringCollider)
        {
            OnTriggerEnter2D(triggeringCollider);
        }
    }
}
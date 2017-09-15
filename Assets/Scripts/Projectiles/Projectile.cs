using General;
using JetBrains.Annotations;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile: MonoBehaviour
    {
        public GameObject TrailPrefab;
        public abstract void SetRange(float range);
        public int Damage;
        public abstract void Destroy();
    }
}
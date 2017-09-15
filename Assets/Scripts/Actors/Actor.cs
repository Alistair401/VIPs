using Helpers;
using UnityEngine;

namespace Actors
{
    public abstract class Actor: MonoBehaviour, IRemoteTrigger
    {
        public int Health;
        public abstract void Trigger(Collider2D triggeringCollider);
    }
}
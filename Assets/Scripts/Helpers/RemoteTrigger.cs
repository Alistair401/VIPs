using UnityEngine;

namespace Helpers
{
    public interface IRemoteTrigger
    {
        void Trigger(Collider2D triggeringCollider);
    }
}
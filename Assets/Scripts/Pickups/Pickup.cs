using System;
using Player;
using UnityEngine;

namespace Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        public abstract void Activate(PlayerInventory playerInventory);

        public abstract string GetName();
    }
}
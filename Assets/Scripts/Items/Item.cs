using Player;
using UnityEngine;

namespace Items
{
    public abstract class Item
    {
        public abstract string GetName();
        
        public abstract void Fire(PlayerActions playerActions);
    }
}
using Player;
using UnityEngine;

namespace Pickups.Weapons
{
    public class Pistol : Pickup
    {
        public GameObject FirePrefab;
        
        public override void Activate(PlayerInventory playerInventory)
        {
            playerInventory.AddItem(new Items.Weapons.Pistol(FirePrefab));
            Destroy(gameObject);
        }

        public override string GetName()
        {
            return "Pistol";
        }
    }
}
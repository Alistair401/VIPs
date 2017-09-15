using System;
using System.Collections.Generic;
using Items;
using Pickups;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private int _cash;
        private List<Item> _inventory;
        private int _heldItem;

        private void Start()
        {
            _inventory = new List<Item>();
            _heldItem = -1;
        }

        public List<Item> GetInventory()
        {
            return _inventory;
        }

        public Item GetHeldItem()
        {
            if (_heldItem == -1) return null;
            return _inventory[_heldItem];
        }

        public Boolean AddItem(Item item)
        {
            // TODO: Inventory limit?
            // TODO: Restricted items?
            // TODO: Duplicate items?
            _inventory.Add(item);
            Debug.Log("Picked up: " + item.GetName());
            if (_inventory.Count == 1)
            {
                _heldItem = 0;
            }
            return true;
        }
    }
}
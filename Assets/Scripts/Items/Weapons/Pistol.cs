using System;
using Player;
using UnityEngine;

namespace Items.Weapons
{
    public class Pistol : Item
    {
        private static float _cooldown = 0.2f;
        private readonly GameObject _firePrefab;
        private float _lastFire;
        private float _range = 200;
        private int _damage = 10;

        public Pistol(GameObject firePrefab)
        {
            _firePrefab = firePrefab;
            _lastFire = -_cooldown;
        }

        public override string GetName()
        {
            return "Pistol";
        }

        public override void Fire(PlayerActions playerActions)
        {
            float currentTime = Time.time;
            if (currentTime - _lastFire > _cooldown)
            {
                playerActions.Fire(_firePrefab, _range, _damage);
                _lastFire = currentTime;
            }
        }
    }
}
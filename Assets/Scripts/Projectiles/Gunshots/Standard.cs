using System.Collections.Generic;
using General;
using Helpers;
using UnityEngine;

namespace Projectiles.Gunshots
{
    public class Standard : Projectile
    {
        private float _range;
        private IActorTransactions _source;
        private SpriteRenderer _spriteRenderer;
        private PolygonCollider2D _collider;


        private List<Trail> _trail;
        private int _trailLength;
        private int _trailCounter;

        private void Start()
        {
            gameObject.layer = 10;
            _collider = gameObject.AddComponent<PolygonCollider2D>();
            _collider.isTrigger = true;

            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Texture2D texture = new Texture2D(6, 6);
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, Color.black);
                }
            }
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            _collider.points = new[] {sprite.vertices[2], sprite.vertices[0], sprite.vertices[1], sprite.vertices[3]};
            _spriteRenderer.sprite = sprite;

            _trail = new List<Trail>();
            _trailLength = 5;
        }

        private void FixedUpdate()
        {
            Vector2 velocity = new Vector2(0, 0.3f);
            Vector2 projectileDirection = transform.rotation * Vector3.up;
            int layerMask = ~(1 << 10 | 1 << 8);
            RaycastHit2D raycastHit2D =
                Physics2D.Raycast(_spriteRenderer.bounds.center, transform.rotation * velocity, velocity.y,
                    layerMask);
            if (raycastHit2D.collider)
            {
                IRemoteTrigger remoteTrigger = raycastHit2D.collider.gameObject.GetComponent<IRemoteTrigger>();
                if (remoteTrigger != null)
                {
                    remoteTrigger.Trigger(_collider);
                    return;
                }
            }
            transform.Translate(velocity);


            if (_trailCounter < _trailLength)
            {
                Vector3 trailDirection = projectileDirection * -1;
                Vector3 nextTrailPosition =
                    transform.position + trailDirection * (_trailCounter + 1) * 0.15f;
                Trail newTrail = Instantiate(
                    TrailPrefab,
                    nextTrailPosition,
                    transform.rotation
                ).gameObject.GetComponent<Trail>();
                newTrail.Velocity = velocity;
                Color nextTrailColor;
                if (_trailCounter == 0)
                {
                    nextTrailColor = Color.red;
                }
                else
                {
                    nextTrailColor = Color.Lerp(_trail[_trailCounter - 1].GetColor(), Color.yellow, 0.2f);
                }
                newTrail.SetColor(nextTrailColor);
                _trail.Add(newTrail);
                _trailCounter++;
            }
        }

        public override void SetRange(float range)
        {
            _range = range;
        }

        public override void Destroy()
        {
            foreach (Trail trail in _trail)
            {
                trail.Limit = transform.position;
                trail.Ending = true;
            }
            Destroy(gameObject);
        }
    }
}
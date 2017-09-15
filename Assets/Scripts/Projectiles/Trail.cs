using System;
using UnityEngine;

namespace Projectiles
{
    public class Trail : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public Vector2 Velocity;
        private Color _color;
        public Vector3 Limit;
        public bool Ending;

        public void SetColor(Color color)
        {
            _color = color;
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            Texture2D texture = new Texture2D(5, 5);
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, color);
                }
            }
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            _spriteRenderer.sprite = sprite;
        }

        public Color GetColor()
        {
            return _color;
        }

        private void FixedUpdate()
        {
            transform.Translate(Velocity);
            if (Ending)
            {
                if (Vector2.Distance(transform.position, Limit) <= 0.2f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
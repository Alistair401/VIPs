using UnityEngine;

namespace Ground.Map
{
    public class Tile
    {
        public const int Empty = 0;
        public const int Road = 1;
        public const int Room = 2;
        public const int Bush = 3;
        public const int Park = 4;
        public const int Door = 5;

        public int Type { get; set; }
        public Vector2 Position { get; set; }
        public int ArrayX { get; set; }
        public int ArrayY { get; set; }
        public bool Visited { get; set; }
        public Room OwningRoom { get; set; }
        public Vector3 Facing { get; set; }
        public bool IsWall { get; set; }
        public bool IsCorner { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Ground.Map
{
    public class Room
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public int Size
        {
            get { return ((MaxX - MinX) * (MaxY - MinY)) ^ 2; }
        }

        private List<Tile> _possibleDoors = new List<Tile>();

        public void AddPossibleDoor(Tile tile)
        {
            _possibleDoors.Add(tile);
        }

        public List<Tile> GetPossibleDoors()
        {
            return _possibleDoors;
        }
    }
}
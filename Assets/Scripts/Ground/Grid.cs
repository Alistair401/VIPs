using System.Collections.Generic;
using UnityEngine;

namespace Ground
{
    public class Grid
    {
        public int ActualXLength;
        public int ActualYLength;

        private List<Vector2> _nodes;

        public Grid(int xLength, int yLength, Vector2 center)
        {
            _nodes = new List<Vector2>();
            int minX = -xLength / 2;
            int maxX = xLength / 2;
            int minY = -yLength / 2;
            int maxY = yLength / 2;

            ActualXLength = maxX - minX + 1;
            ActualYLength = maxY - minY + 1;

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    _nodes.Add(new Vector2(i + center.x, j + center.y));
                }
            }
        }

        public List<Vector2> GetNodes()
        {
            return _nodes;
        }
    }
}
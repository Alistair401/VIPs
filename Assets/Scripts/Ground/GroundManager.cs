using Ground.Map;
using UnityEngine;
using UnityEngine.Networking;

namespace Ground
{
    public class GroundManager : NetworkBehaviour
    {
        public int Scale;
        public GameObject WallPrefab;

        private Grid _grid;
        private GroundMesh _groundMesh;
        private CityMap _cityMap;

        // Use this for initialization
        void Start()
        {
            _grid = new Grid(Scale, Scale, new Vector2(0, 0));
            _cityMap = new CityMap(_grid);
            _groundMesh = gameObject.GetComponent<GroundMesh>();
            foreach (Tile tile in _cityMap.GetTiles())
            {
                _groundMesh.AddTile(tile);
                if (tile.Type == Tile.Room && tile.IsWall)
                {
                    if (tile.IsCorner)
                    {
                        Quaternion newRotation = Quaternion.LookRotation(tile.Facing, Vector3.back);
                        newRotation.x = 0;
                        newRotation.y = 0;
                        Instantiate(WallPrefab, tile.Position, newRotation);
                        if (tile.Facing.x == 0)
                        {
                            newRotation = Quaternion.LookRotation(new Vector2(tile.Facing.y, tile.Facing.x) * -1,
                                Vector3.back);
                        }
                        else
                        {
                            newRotation = Quaternion.LookRotation(new Vector2(tile.Facing.y, tile.Facing.x),
                                Vector3.back);
                        }
                        newRotation.x = 0;
                        newRotation.y = 0;
                        Instantiate(WallPrefab, tile.Position, newRotation);
                    }
                    else
                    {
                        Quaternion newRotation = Quaternion.LookRotation(tile.Facing, Vector3.back);
                        newRotation.x = 0;
                        newRotation.y = 0;
                        Instantiate(WallPrefab, tile.Position, newRotation);
                    }
                }
            }
        }
    }
}
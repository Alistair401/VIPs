using System.Collections.Generic;
using Ground.Map;
using UnityEngine;

namespace Ground
{
    public class GroundMesh : MonoBehaviour
    {
        private MeshFilter _mf;
        private MeshRenderer _mr;
        private Mesh _mesh;
        public Material Material;

        private Queue<Tile> _tilesToAdd = new Queue<Tile>();

        private void Start()
        {
            _mf = gameObject.AddComponent<MeshFilter>();
            _mr = gameObject.AddComponent<MeshRenderer>();
            _mesh = new Mesh();
            _mf.mesh = _mesh;
            _mr.material = Material;
        }

        public void AddTile(Tile tile)
        {
            _tilesToAdd.Enqueue(tile);
        }

        private void Update()
        {
            if (_tilesToAdd.Count <= 0) return;
            List<Vector3> concatenatedVertices = new List<Vector3>();
            concatenatedVertices.AddRange(_mesh.vertices);

            List<int> concatenatedTriangles = new List<int>();
            concatenatedTriangles.AddRange(_mesh.triangles);

            List<Vector2> concatenatedUVs = new List<Vector2>();
            concatenatedUVs.AddRange(_mesh.uv);

            while (_tilesToAdd.Count > 0)
            {
                Tile nextTile = _tilesToAdd.Dequeue();
                Vector3 tileCenter = nextTile.Position;

                Vector3[] newVertices =
                {
                    new Vector3(tileCenter.x - 0.5f, tileCenter.y - 0.5f, 0),
                    new Vector3(tileCenter.x + 0.5f, tileCenter.y - 0.5f, 0),
                    new Vector3(tileCenter.x - 0.5f, tileCenter.y + 0.5f, 0),
                    new Vector3(tileCenter.x + 0.5f, tileCenter.y + 0.5f, 0)
                };

                int[] newTriangles = {0, 3, 1, 0, 2, 3};
                for (int i = 0; i < newTriangles.Length; i++)
                {
                    newTriangles[i] = newTriangles[i] + concatenatedVertices.Count;
                }

                int offset = 0;
                switch (nextTile.Type)
                {
                    case Tile.Road:
                    {
                        offset = 1;
                        break;
                    }
                    case Tile.Room:
                    {
                        offset = 2;
                        break;
                    }
                    case Tile.Door:
                    {
                        offset = 3;
                        break;
                    }
                }

                Vector2[] newUVs =
                {
                    new Vector2(offset * 64f / 320f, 0),
                    new Vector2((offset + 1) * 64f / 320f, 0),
                    new Vector2(offset * 64f / 320f, 1),
                    new Vector2((offset + 1) * 64f / 320f, 1),
                };

                concatenatedUVs.AddRange(newUVs);
                concatenatedVertices.AddRange(newVertices);
                concatenatedTriangles.AddRange(newTriangles);
            }

            _mesh.vertices = concatenatedVertices.ToArray();
            _mesh.triangles = concatenatedTriangles.ToArray();
            _mesh.uv = concatenatedUVs.ToArray();

            _mesh.RecalculateNormals();
        }
    }
}
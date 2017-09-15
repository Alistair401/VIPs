using UnityEngine;

namespace Debugging
{
    public class DisplayChunks : MonoBehaviour
    {
        public float XDist;
        public float ZDist;
        public float ChunkSize;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnDrawGizmos()
        {
            if (XDist > 0 && ZDist > 0 && ChunkSize > 0)
            {
                for (float i = 0; i <= XDist / ChunkSize; i += 1)
                {
                    Gizmos.DrawRay(new Vector3(i * ChunkSize, 0, 0), new Vector3(0, 0, ZDist));
                }
                for (float i = 0; i <= ZDist / ChunkSize; i += 1)
                {
                    Gizmos.DrawRay(new Vector3(0, 0, i * ChunkSize), new Vector3(XDist, 0, 0));
                }
            }
        }
    }
}
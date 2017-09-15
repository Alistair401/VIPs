using UnityEngine;

namespace Camera
{
    public class CameraControl : MonoBehaviour
    {
        private CameraManager _manager;

        // Use this for initialization
        void Start()
        {
            _manager = gameObject.GetComponent<CameraManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_manager.Camera == null || _manager.GetSubject() == null) return;
            if (_manager.Camera.orthographic)
            {
                _manager.Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 2;
            }
            else
            {
                _manager.Camera.transform.position += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * 2);
            }
            _manager.Camera.transform.position = new Vector3(
                _manager.GetSubject().transform.position.x,
                _manager.GetSubject().transform.position.y,
                _manager.Camera.transform.position.z
            );
        }
    }
}
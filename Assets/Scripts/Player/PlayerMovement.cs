using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Vector3 LookDirection;
        private PlayerManager _playerManager;
        private Plane _mousePlane;

        private void Start()
        {
            _playerManager = gameObject.GetComponent<PlayerManager>();
            _mousePlane = new Plane(Vector3.forward, Vector3.zero);
        }

        private void FixedUpdate()
        {
            gameObject.transform.position = new Vector2(
                gameObject.transform.position.x + Input.GetAxis("Horizontal") * _playerManager.Speed,
                gameObject.transform.position.y + Input.GetAxis("Vertical") * _playerManager.Speed);
        }

        private void Update()
        {
            if (!_playerManager.Camera) return;
            Vector3 mousePosition;
            if (_playerManager.Camera.orthographic)
            {
                mousePosition = _playerManager.Camera.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Ray mouseRay = _playerManager.Camera.ScreenPointToRay(Input.mousePosition);
                float planeInterceptDistance;
                _mousePlane.Raycast(mouseRay, out planeInterceptDistance);
                mousePosition = mouseRay.GetPoint(planeInterceptDistance);
            }
            LookDirection = gameObject.transform.position - mousePosition;
            Quaternion newRotation = Quaternion.LookRotation(LookDirection, Vector3.forward);
            newRotation.x = 0;
            newRotation.y = 0;
            newRotation = Quaternion.Lerp(newRotation, gameObject.transform.rotation, 0.6f);
            gameObject.transform.rotation = newRotation;
        }
    }
}
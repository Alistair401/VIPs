using UnityEngine;

namespace Camera
{
	public class CameraManager : MonoBehaviour
	{
		public UnityEngine.Camera Camera;
		public GameObject Subject;

		// Use this for initialization
		void Start ()
		{
			gameObject.AddComponent<CameraControl>();
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public GameObject GetSubject()
		{
			return Subject;
		}
	}
}

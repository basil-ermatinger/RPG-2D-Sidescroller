using UnityEngine;

namespace Rpg2dSidescroller
{
  public class ParallaxBackground : MonoBehaviour
  {
    private Camera _mainCamera;
		private float _lastCameraPositionX;

    [SerializeField] private ParallaxLayer[] _backgroundLayers;

		private void Awake()
		{
			_mainCamera = Camera.main;
		}

		private void Update()
		{
			float currentCameraPositionX = _mainCamera.transform.position.x;
			float distanceToMove = currentCameraPositionX - _lastCameraPositionX;
			_lastCameraPositionX = currentCameraPositionX;

			foreach(ParallaxLayer layer in _backgroundLayers)
			{
				layer.Move(distanceToMove);
			}
		}
	}
}

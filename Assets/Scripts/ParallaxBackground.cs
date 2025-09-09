using UnityEngine;

namespace Rpg2dSidescroller
{
  public class ParallaxBackground : MonoBehaviour
  {
    private Camera _mainCamera;
		private float _lastCameraPositionX;
		private float _cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] _backgroundLayers;

		private void Awake()
		{
			_mainCamera = Camera.main;
			_cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;

			InitializeLayers();
		}

		private void FixedUpdate()
		{
			float currentCameraPositionX = _mainCamera.transform.position.x;
			float distanceToMove = currentCameraPositionX - _lastCameraPositionX;
			_lastCameraPositionX = currentCameraPositionX;

			float cameraLeftEdge = currentCameraPositionX - _cameraHalfWidth;
			float cameraRightEdge = currentCameraPositionX + _cameraHalfWidth;

			foreach(ParallaxLayer layer in _backgroundLayers)
			{
				layer.Move(distanceToMove);
				layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
			}
		}

		private void InitializeLayers()
		{
			foreach(ParallaxLayer layer in _backgroundLayers)
			{
				layer.CalculcateImageWidth();
			}
		}
	}
}

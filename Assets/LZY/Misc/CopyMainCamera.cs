using UnityEngine;

namespace LZY.Utilities
{
    [ExecuteInEditMode]
    public class CopyMainCamera : MonoBehaviour
    {
        private Camera _currentCamera;
        private Camera _mainCamera;

        private void Awake()
        {
            _currentCamera = GetComponent<Camera>();
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_currentCamera == null || _mainCamera == null) return;

            _currentCamera.transform.position = _mainCamera.transform.position;
            _currentCamera.transform.rotation = _mainCamera.transform.rotation;

            _currentCamera.orthographic = _mainCamera.orthographic;
            _currentCamera.fieldOfView = _mainCamera.fieldOfView;
            _currentCamera.orthographicSize = _mainCamera.orthographicSize;
            _currentCamera.nearClipPlane = _mainCamera.nearClipPlane;
            _currentCamera.farClipPlane = _mainCamera.farClipPlane;
        }
    }
}
using UnityEngine;

namespace LZY.Utilities
{
    [ExecuteInEditMode]
    public class LookAtMainCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_mainCamera == null) return;
            
            var directionToCamera = _mainCamera.transform.position - transform.position;
            directionToCamera.y = 0f;
            if (directionToCamera.sqrMagnitude < 0.0001f) return;
            
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }
   
}
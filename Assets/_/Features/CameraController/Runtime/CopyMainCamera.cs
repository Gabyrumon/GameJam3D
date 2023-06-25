using UnityEngine;

namespace CameraController.Runtime
{
    public class CopyMainCamera : MonoBehaviour
    {
        #region Public Members

        private void Awake()
        {
            _mainCamera = Camera.main;
            _currentCamera = GetComponent<Camera>();
        }

        #endregion

        #region Unity API

        private void LateUpdate()
        {
            _currentCamera.fieldOfView = _mainCamera.fieldOfView;
        }

        #endregion

        #region Private and Protected Members

        private Camera _mainCamera;
        private Camera _currentCamera;

        #endregion
    }
}
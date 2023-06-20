using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUD.Runtime
{
    public class HUDLookAtCamera : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
        }

        #endregion

        #region Main Methods

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private Camera _camera;

        #endregion
    }
}
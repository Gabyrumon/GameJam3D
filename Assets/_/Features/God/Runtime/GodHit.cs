using Inputs.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace God.Runtime
{
    public class GodHit : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _inputManager.m_onMouseMove += OnMouseMoveEventHandler;
            _inputManager.m_onHit += OnHitEventHandler;
        }

        private void OnDisable()
        {
            _inputManager.m_onMouseMove -= OnMouseMoveEventHandler;
            _inputManager.m_onHit -= OnHitEventHandler;
        }

        #endregion

        #region Main Methods

        private void OnMouseMoveEventHandler(object sender, OnMouseMoveEventArgs e)
        {
            _mousePosition = e.m_mousePosition;
        }

        private void OnHitEventHandler(object sender, EventArgs e)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(_mousePosition), out RaycastHit hit))
            {
                Instantiate(_hitSphere, hit.point, Quaternion.identity);
            }
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private GameObject _hitSphere;

        private InputManager _inputManager;
        private Camera _camera;

        private Vector2 _mousePosition;

        #endregion
    }
}
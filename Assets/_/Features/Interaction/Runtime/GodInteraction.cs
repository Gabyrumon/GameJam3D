using Inputs.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction.Runtime
{
    public class GodInteraction : MonoBehaviour
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
            InputManager.m_instance.m_onInteraction += OnInteractionEventHandler;
            InputManager.m_instance.m_onMouseMove += OnMouseMoveEventHandler;
        }

        private void OnDisable()
        {
            InputManager.m_instance.m_onInteraction -= OnInteractionEventHandler;
            InputManager.m_instance.m_onMouseMove -= OnMouseMoveEventHandler;
        }

        private void OnMouseMoveEventHandler(object sender, OnMouseMoveEventArgs e)
        {
            _mousePosition = e.m_mousePosition;
        }

        private void OnInteractionEventHandler(object sender, EventArgs e)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay((Vector3)_mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out InteractionCircle interactionCircle))
                {
                    OpenInteractionCircle(interactionCircle);
                }
            }
            else
            {
                CloseInteractionCircle();
            }
        }

        #endregion

        #region Main Methods

        private void OpenInteractionCircle(InteractionCircle interactionCircle)
        {
            _currentInteractionCircle = interactionCircle;
            _currentInteractionCircle.Open();
        }

        private void CloseInteractionCircle()
        {
            _currentInteractionCircle.Close();
            _currentInteractionCircle = null;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private Camera _camera;

        private Vector2 _mousePosition;

        private InteractionCircle _currentInteractionCircle;

        #endregion
    }
}
using GodRessources.Runtime;
using Inputs.Runtime;
using System;
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
            if (Physics.Raycast(_camera.ScreenPointToRay(_mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out ObjectInteraction objectInteraction))
                {
                    if (objectInteraction is WalkToDivineIntervention && (objectInteraction as WalkToDivineIntervention).m_isActive)
                    {
                        _currentInteraction = objectInteraction;
                    }
                }
                // When a WalkToDivineIntervention is currently selected.
                else if (_currentInteraction is WalkToDivineIntervention && hit.collider.TryGetComponent(out DivineIntervention divineIntervention))
                {
                    ManageWalkToDivineIntervention(divineIntervention);
                }
                else
                {
                    _currentInteraction = null;
                }
            }
            else
            {
                _currentInteraction = null;
            }
        }

        #endregion

        #region Main Methods

        private void ManageWalkToDivineIntervention(DivineIntervention divineIntervention)
        {
            _currentInteraction = null;
            if (divineIntervention.m_faithOrbCost > _godInventory.m_faithOrbCount)
            {
                return;
            }
            (_currentInteraction as WalkToDivineIntervention).m_pointOfInterest = divineIntervention;
            _currentInteraction.PlayInteraction();
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private GodInventory _godInventory;

        private Camera _camera;

        private Vector2 _mousePosition;

        private ObjectInteraction _currentInteraction;

        #endregion
    }
}
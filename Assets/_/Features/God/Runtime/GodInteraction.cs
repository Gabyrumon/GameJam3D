using ChurchFeature.Runtime;
using Inputs.Runtime;
using Interaction.Runtime;
using System;
using UnityEngine;
using Villager.Runtime;

namespace God.Runtime
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
                if (hit.collider.TryGetComponent(out VillagerAI villagerAI))
                {
                    if (!villagerAI.m_isSelected)
                    {
                        SelectVillager(villagerAI);
                    }
                }
                // When a WalkToDivineIntervention is currently selected.
                else if (villagerAI is not null && hit.collider.TryGetComponent(out DivineIntervention divineIntervention))
                {
                    ManageWalkToDivineIntervention(divineIntervention);
                }
                else
                {
                    UnselectVillager();
                }
            }
            else
            {
                UnselectVillager();
            }
        }

        #endregion

        #region Main Methods

        private void ManageWalkToDivineIntervention(DivineIntervention divineIntervention)
        {
            _currentVillager.m_divineIntervention = divineIntervention;
            _currentVillager.ChangeState(VillagerAI.VillagerState.BarrelAction);

            UnselectVillager();
        }

        private void SelectVillager(VillagerAI villagerAI)
        {
            _currentVillager = villagerAI;
        }

        private void UnselectVillager()
        {
            _currentVillager = null;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private Church _church;

        private Camera _camera;

        private Vector2 _mousePosition;

        private VillagerAI _currentVillager;

        #endregion
    }
}
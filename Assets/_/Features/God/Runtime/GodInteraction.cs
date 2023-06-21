using ChurchFeature.Runtime;
using Inputs.Runtime;
using System;
using UnityEngine;
using Villager.Runtime;
using ExternalOutline;

namespace God.Runtime
{
    public class GodInteraction : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Start()
        {
            _camera = Camera.main;
            _church = Church.m_instance;
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
                    SelectVillager(villagerAI);
                }
                // When a villager is currently selected.
                else if (_currentVillager is not null && hit.collider.TryGetComponent(out DivineIntervention divineIntervention))
                {
                    HandleDivineIntervention(divineIntervention);
                }
                else
                {
                    UnselectVillager(true);
                }
            }
            else
            {
                UnselectVillager(true);
            }
        }

        #endregion

        #region Main Methods

        private void SelectVillager(VillagerAI villagerAI)
        {
            if (villagerAI.CurrentState != VillagerAI.VillagerState.Routine) return;

            if (_currentVillager is not null)
            {
                UnselectVillager(true);
            }

            _currentVillager = villagerAI;
            _currentVillager.ChangeState(VillagerAI.VillagerState.Idle);

            _currentVillager.GetComponent<Outline>().enabled = true;
            foreach (var divineIntervention in _divineInterventions)
            {
                if (!CanPlayDivineInteraction(divineIntervention)) break;

                divineIntervention.GetComponent<Outline>().enabled = true;
            }
        }

        private void UnselectVillager(bool returnToRoutine)
        {
            if (returnToRoutine)
            {
                _currentVillager?.ReturnToRoutine();
            }

            if (_currentVillager is null) return;

            _currentVillager.GetComponent<Outline>().enabled = false;
            _currentVillager = null;

            foreach (var divineIntervention in _divineInterventions)
            {
                divineIntervention.GetComponent<Outline>().enabled = false;
            }
        }

        private void HandleDivineIntervention(DivineIntervention divineIntervention)
        {
            if (!CanPlayDivineInteraction(divineIntervention))
            {
                UnselectVillager(true);
                return;
            }

            _currentVillager.m_divineIntervention = divineIntervention;
            _currentVillager.ChangeState(VillagerAI.VillagerState.BarrelAction);

            _church.FaithOrbCount -= divineIntervention.OrbCost;
            divineIntervention.IsInteractable = false;

            UnselectVillager(false);
        }

        private bool CanPlayDivineInteraction(DivineIntervention divineIntervention)
        {
            return _church.FaithOrbCount >= divineIntervention.OrbCost
                && _church.m_level >= divineIntervention.RequiredChurchLevel
                && divineIntervention.IsInteractable;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private DivineIntervention[] _divineInterventions;

        private Church _church;

        private Camera _camera;

        private Vector2 _mousePosition;

        private VillagerAI _currentVillager;

        #endregion
    }
}
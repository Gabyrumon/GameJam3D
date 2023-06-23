using ChurchFeature.Runtime;
using Inputs.Runtime;
using System;
using UnityEngine;
using Villager.Runtime;
using ExternalOutline;
using Sound.Runtime;

namespace God.Runtime
{
    public class GodInteraction : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

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

        private void Start()
        {
            _camera = Camera.main;
            _church = Church.m_instance;
        }

        private void Update()
        {
            CheckIfDivineInterventionsBecomeAvailable();
        }

        #endregion

        #region Main Methods

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

        private void CheckIfDivineInterventionsBecomeAvailable()
        {
            if (_currentVillager is null) return;

            foreach (var divineIntervention in _divineInterventions)
            {
                Outline currentOutline = divineIntervention.GetComponent<Outline>();
                if (currentOutline.OutlineColor.Equals(_lockedOutlineColor) && CanPlayDivineInteraction(divineIntervention))
                {
                    currentOutline.OutlineColor = _unlockedOutlineColor;
                }
            }
        }

        private void SelectVillager(VillagerAI villagerAI)
        {
            if (!villagerAI.IsConverted || villagerAI.CurrentState == VillagerAI.VillagerState.BarrelAction) return;

            if (_currentVillager is not null)
            {
                UnselectVillager(true);
            }

            _currentVillager = villagerAI;

            bool isPraying = false;
            if (_currentVillager.GetState() == VillagerAI.VillagerState.Pray)
            {
                isPraying = true;
                Prayer prayer = _currentVillager.GetComponentInChildren<Prayer>();
                if (prayer.transform.parent.gameObject.activeSelf)
                {
                    prayer.ValidatePrayer();
                }
            }
            _currentVillager.ChangeState(VillagerAI.VillagerState.Idle);
            if (isPraying) _currentVillager.TimeBeforePray = 1;

            SoundManager.m_instance.PlayVillagerVoiceInterrogative(_currentVillager.IsMan);

            _currentVillager.GetComponent<Outline>().enabled = true;
            foreach (var divineIntervention in _divineInterventions)
            {
                if (!CanPlayDivineInteraction(divineIntervention, false, false)) break;

                divineIntervention.GetComponent<Outline>().OutlineColor =
                    _church.FaithOrbCount >= divineIntervention.OrbCost && divineIntervention.IsInteractable
                    ? _unlockedOutlineColor : _lockedOutlineColor;

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

            SoundManager.m_instance.PlayVillagerVoiceYes(_currentVillager.IsMan);
            SoundManager.m_instance.PlayGodWhisper();

            UnselectVillager(false);
        }

        private bool CanPlayDivineInteraction(DivineIntervention divineIntervention, bool checkIsInteractable = true, bool checkOrbs = true)
        {
            bool result = _church.m_level >= divineIntervention.RequiredChurchLevel;

            if (checkIsInteractable)
            {
                result = result && divineIntervention.IsInteractable;
            }
            if (checkOrbs)
            {
                result = result && _church.FaithOrbCount >= divineIntervention.OrbCost;
            }
            return result;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private DivineIntervention[] _divineInterventions;

        [SerializeField] private Color _unlockedOutlineColor;
        [SerializeField] private Color _lockedOutlineColor;

        private Church _church;

        private Camera _camera;

        private Vector2 _mousePosition;

        private VillagerAI _currentVillager;

        #endregion
    }
}
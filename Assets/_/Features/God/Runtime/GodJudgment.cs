using System;
using ChurchFeature.Runtime;
using Inputs.Runtime;
using Sound.Runtime;
using UnityEngine;
using Utils.Runtime;
using Villager.Runtime;

namespace God.Runtime
{
    public class GodJudgment : MonoBehaviour
    {
        #region Public Members

        public EventHandler<bool> m_onJudgmentReady;

        #endregion
        
        #region Unity API

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            InputManager.m_instance.m_onMouseMove -= OnMouseMoveEventHandler;
            InputManager.m_instance.m_onJudgment -= OnJudgmentEventHandler;

            ChurchManager.Instance.m_onFaithChanged -= OnFaithChangedEventHandler;

            SatanManager.m_instance.m_onGoWinTheGameEventHandler -= OnGoWinTheGameEventHandler;
        }

        private void Start()
        {
            SubscribeToEvents();
            m_onJudgmentReady?.Invoke(this, IsJudgmentReady());
        }

        #endregion

        #region Main Methods

        private void SubscribeToEvents()
        {
            InputManager.m_instance.m_onMouseMove += OnMouseMoveEventHandler;
            InputManager.m_instance.m_onJudgment += OnJudgmentEventHandler;

            ChurchManager.Instance.m_onFaithChanged += OnFaithChangedEventHandler;

            SatanManager.m_instance.m_onGoWinTheGameEventHandler += OnGoWinTheGameEventHandler;
        }

        private void OnMouseMoveEventHandler(object sender, OnMouseMoveEventArgs e)
        {
            _mousePosition = e.m_mousePosition;
        }

        private void OnJudgmentEventHandler(object sender, EventArgs e)
        {
            if (!IsJudgmentReady() || Mouse.IsMouseOverUI(_mousePosition)) return;

            if (Physics.Raycast(_camera.ScreenPointToRay(_mousePosition), out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer != 1 << LayerMask.NameToLayer("UI"))
                {
                    SoundManager.m_instance.PlayJudgment();
                    Instantiate(_judgmentPrefab, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity);
                }
            }
            ChurchManager.Instance.FaithCount -= _judgmentCost;
        }

        private void OnFaithChangedEventHandler(object sender, OnFaithChangedEventArgs e)
        {
            m_onJudgmentReady?.Invoke(this, IsJudgmentReady());
        }

        private bool IsJudgmentReady()
        {
            return ChurchManager.Instance.Level >= _levelRequiredForJudgment && ChurchManager.Instance.FaithCount >= _judgmentCost;
        }

        private void OnGoWinTheGameEventHandler(object sender, EventArgs e)
        {
            _judgmentCost = 0;
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private GameObject _judgmentPrefab;

        [Space]
        [SerializeField] private int _judgmentCost;
        [SerializeField] private int _levelRequiredForJudgment;
        
        private Camera _camera;

        private Vector2 _mousePosition;

        #endregion
    }
}
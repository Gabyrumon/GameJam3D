using ChurchFeature.Runtime;
using Inputs.Runtime;
using Sound.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace God.Runtime
{
    public class GodJudgment : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            _camera = Camera.main;
            _inputManager = InputManager.m_instance;
        }

        private void Start()
        {
            _churchManager = ChurchManager.Instance;
        }

        private void OnEnable()
        {
            _inputManager.m_onMouseMove += OnMouseMoveEventHandler;
            _inputManager.m_onJudgment += OnJudgmentEventHandler;
        }

        private void OnDisable()
        {
            _inputManager.m_onMouseMove -= OnMouseMoveEventHandler;
            _inputManager.m_onJudgment -= OnJudgmentEventHandler;
        }

        #endregion

        #region Main Methods

        private void OnMouseMoveEventHandler(object sender, OnMouseMoveEventArgs e)
        {
            _mousePosition = e.m_mousePosition;
        }

        private void OnJudgmentEventHandler(object sender, EventArgs e)
        {
            if (!_churchManager.IsJudgmentReady() || IsMouseOverUI()) return;

            if (Physics.Raycast(_camera.ScreenPointToRay(_mousePosition), out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer != _uiLayer)
                {
                    SoundManager.m_instance.PlayJudgment();
                    Instantiate(_judgmentPrefab, new Vector3(hit.point.x, 0, hit.point.z), Quaternion.identity);
                }
            }
            _churchManager.FaithCount -= _churchManager.JudgmentCost;
        }

        #endregion

        #region Utils

        private bool IsMouseOverUI()
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ScreenPosToPointerData(_mousePosition), results);
            return results.Count > 0;
        }

        private PointerEventData ScreenPosToPointerData(Vector2 screenPos)
           => new(EventSystem.current) { position = screenPos };

        #endregion

        #region Private and Protected Members

        [SerializeField] private GameObject _judgmentPrefab;
        [SerializeField] private LayerMask _uiLayer;

        private ChurchManager _churchManager;
        private InputManager _inputManager;
        private Camera _camera;

        private Vector2 _mousePosition;

        #endregion
    }
}
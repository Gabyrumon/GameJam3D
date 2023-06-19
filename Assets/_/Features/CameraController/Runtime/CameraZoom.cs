using UnityEngine;
using Inputs.Runtime;
using System;

namespace CameraController.Runtime
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            InputManager.m_instance.m_onZoom += OnZoomEventHandler;

            _baseZoom = _camera.fieldOfView;
        }

        private void Update()
        {
            if (_deltaZoom < 1)
            {
                _deltaZoom += Time.deltaTime / _timeToZoom;
                _camera.fieldOfView = Mathf.Lerp(_startingZoom, _targetZoom, _deltaZoom);
            }
        }

        private void OnZoomEventHandler(object sender, EventArgs e)
        {
            Toogle();
        }

        private void Toogle()
        {
            _startingZoom = _camera.fieldOfView;
            _deltaZoom = 0;

            if (!_toggle)
            {
                _targetZoom = _baseZoom * _intensity;

                _cameraRotation.MultiplySensitivity(_sensitivityMultiplier);
                _cameraRotation.SetBaseLookPosition(false);
            }
            else
            {
                _targetZoom = _baseZoom;

                _cameraRotation.MultiplySensitivity(1 / _sensitivityMultiplier);
                _cameraRotation.SetBaseLookPosition(true);
            }

            _toggle = !_toggle;
        }

        [SerializeField] private CameraRotation _cameraRotation;

        [Space]

        [Range(0, 1)]
        [SerializeField] private float _intensity = 0.5f;

        [Range(0.01f, 1f)]
        [SerializeField] private float _timeToZoom;

        [Tooltip("The sensitivity multiplier when zooming")] [Range(0, 1)]
        [SerializeField] private float _sensitivityMultiplier = 0.5f;


        private Camera _camera;
        private bool _toggle;

        private float _baseZoom;
        private float _startingZoom;
        private float _targetZoom;
        private float _deltaZoom = 1;
    }
}
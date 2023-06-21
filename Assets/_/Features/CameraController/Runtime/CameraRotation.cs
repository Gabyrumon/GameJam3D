using System;
using UnityEngine;
using Inputs.Runtime;

namespace CameraController.Runtime
{
    public class CameraRotation : MonoBehaviour
    {
        private void Awake()
        {
            _baseRotation = transform.localEulerAngles;
            _currentRotation = _baseRotation;
        }

        private void Start()
        {
            InputManager.m_instance.m_onMouseMove += OnMouseMoveEventHandler;
        }


        void Update()
        {
            PivotTowardMouse();
        }

        private void PivotTowardMouse()
        {
            Vector2 normalizedMousPosition;
            normalizedMousPosition.x = Mathf.Clamp(_mousePos.x / Screen.width, _verticalDeadZone.m_lowerX, _verticalDeadZone.m_upperX);
            normalizedMousPosition.y = Mathf.Clamp(_mousePos.y / Screen.height, _verticalDeadZone.m_lowerY, _verticalDeadZone.m_upperY);

            Vector3 destinationRotation = new Vector3(
                _currentRotation.x - (normalizedMousPosition.y - 0.5f) * _sensitivity,
                _currentRotation.y + (normalizedMousPosition.x - 0.5f) * _sensitivity,
                _currentRotation.z);

            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, destinationRotation, _rotationSpeed * Time.deltaTime);
        }


        private void OnMouseMoveEventHandler(object sender, OnMouseMoveEventArgs e)
        {
            _mousePos = e.m_mousePosition;
        }

        public void MultiplySensitivity(float multiplier)
        {
            _sensitivity *= multiplier;
        }

        public void SetBaseLookPosition(bool boolean = true)
        {
            if (boolean)
            {
                _currentRotation = _baseRotation;
            }
            else
            {
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
                _currentRotation = Quaternion.LookRotation(hit.point - transform.position).eulerAngles;
            }
        }

        [Range(0, 10)]
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private float _sensitivity = 50;
        [Space]

        [Tooltip("Uses normalize values")]
        [SerializeField] private DeadZone _verticalDeadZone;

        private Vector3 _currentRotation;
        private Vector3 _baseRotation;
        private Vector2 _mousePos;
    }

    [Serializable]
    public class DeadZone
    {
        [Range(0, 1)] public float m_upperX = 1;
        [Range(0, 1)] public float m_lowerX;
        [Space]
        [Range(0, 1)] public float m_upperY = 1;
        [Range(0, 1)] public float m_lowerY;
    }
}
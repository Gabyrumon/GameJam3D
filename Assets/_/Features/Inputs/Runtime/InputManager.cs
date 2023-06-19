using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs.Runtime
{
    public class OnMouseMoveEventArgs : EventArgs
    {
        public Vector2 m_mousePosition;
    }

    public class OnMoveEventArgs : EventArgs
    {
        public Vector2 m_direction;
    }


    public class InputManager : MonoBehaviour
    {
        public static InputManager m_instance;
        public PlayerInput m_playerInput;

        public EventHandler<OnMouseMoveEventArgs> m_onMouseMove;
        public EventHandler<OnMoveEventArgs> m_onMove;
        public EventHandler m_onInteraction;
        public EventHandler m_onZoom;
        public EventHandler m_onPauseMenu;


        private void Awake()
        {
            m_instance = this;
        }


        public void OnMouseMoveEventHandler(InputAction.CallbackContext context)
        {
            m_onMouseMove?.Invoke(this, new OnMouseMoveEventArgs() { m_mousePosition = context.ReadValue<Vector2>() });
        }

        public void OnMoveEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            m_onMove?.Invoke(this, new OnMoveEventArgs() { m_direction = context.ReadValue<Vector2>() });
        }

        public void OnInteractionEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            m_onInteraction?.Invoke(this, new EventArgs());
        }

        public void OnZoomEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            m_onZoom?.Invoke(this, new EventArgs());
        }

        public void OnPauseEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            m_onPauseMenu?.Invoke(this, new EventArgs());
        }
    }
}
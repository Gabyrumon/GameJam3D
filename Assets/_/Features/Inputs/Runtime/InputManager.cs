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

        public bool m_cantInteract;

        public EventHandler<OnMouseMoveEventArgs> m_onMouseMove;
        public EventHandler m_onInteraction;
        public EventHandler m_onHit;
        public EventHandler m_onJudgment;
        public EventHandler m_onZoom;
        public EventHandler m_onPauseMenu;

        private void Awake()
        {
            m_instance = this;
        }

        public void OnMouseMoveEventHandler(InputAction.CallbackContext context)
        {
            m_onMouseMove?.Invoke(this, new OnMouseMoveEventArgs { m_mousePosition = context.ReadValue<Vector2>() });
        }

        public void OnInteractionEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started || Time.timeScale == 0 || m_cantInteract) return;

            m_onInteraction?.Invoke(this, new EventArgs());
        }

        public void OnHitEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started || Time.timeScale == 0 || m_cantInteract) return;

            m_onHit?.Invoke(this, new EventArgs());
        }

        public void OnJudgmentEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started || Time.timeScale == 0 || m_cantInteract) return;

            m_onJudgment?.Invoke(this, new EventArgs());
        }

        public void OnZoomEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started || Time.timeScale == 0 || m_cantInteract) return;

            m_onZoom?.Invoke(this, new EventArgs());
        }

        public void OnPauseEventHandler(InputAction.CallbackContext context)
        {
            if (!context.started || m_cantInteract) return;

            m_onPauseMenu?.Invoke(this, new EventArgs());
        }
    }
}
using Inputs.Runtime;
using Sound.Runtime;
using System;
using UnityEngine;

namespace GameManagerFeature.Runtime
{
    public class GameManager : MonoBehaviour
    {
        #region Public Members

        public static GameManager m_instance;

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance != null) return;

            m_instance = this;

            _inputManager = InputManager.m_instance;
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            _inputManager.m_onPauseMenu += OnPauseEventHandler;
        }

        private void OnDisable()
        {
            _inputManager.m_onPauseMenu -= OnPauseEventHandler;
        }

        #endregion

        #region Main Methods

        private void OnPauseEventHandler(object sender, EventArgs e)
        {
            TogglePause();
        }

        public void TogglePause(bool usePauseMenu = true)
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            if (Time.timeScale == 0)
            {
                SoundManager.m_instance.PlayPause();
                SoundManager.m_instance.SetPausedInGameMusic(true);
            }
            else
            {
                SoundManager.m_instance.PlayUnpause();
                SoundManager.m_instance.SetPausedInGameMusic(false);
            }
            if (usePauseMenu) _pauseMenu.Toggle();
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private PauseMenu _pauseMenu;

        private InputManager _inputManager;

        #endregion
    }
}
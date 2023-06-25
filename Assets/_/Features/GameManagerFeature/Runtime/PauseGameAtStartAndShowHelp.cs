using Inputs.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerFeature.Runtime
{
    public class PauseGameAtStartAndShowHelp : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            GameManager.m_instance.TogglePause(false);

            InputManager.m_instance.m_onPauseMenu += OnPauseEventHandler;
        }

        #endregion

        #region Main Methods

        private void OnPauseEventHandler(object sender, EventArgs e)
        {
            if (gameObject.activeSelf)
            {
                ResumeGame();
            }
        }

        public void ResumeGame()
        {
            GameManager.m_instance.TogglePause(false);
            gameObject.SetActive(false);
        }

        #endregion
    }
}
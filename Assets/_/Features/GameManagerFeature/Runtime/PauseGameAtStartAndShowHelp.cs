using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerFeature.Runtime
{
    public class PauseGameAtStartAndShowHelp : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Awake()
        {
            GameManager.m_instance.TogglePause();
        }

        #endregion

        #region Main Methods

        public void ResumeGame()
        {
            GameManager.m_instance.TogglePause(false);
            gameObject.SetActive(false);
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        #endregion
    }
}
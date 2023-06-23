using JetBrains.Annotations;
using Sound.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager.Runtime
{
    public class PauseMenu : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Start()
        {
            _gameManager = GameManager.m_instance;
        }

        #endregion

        #region Main Methods

        public void ToggleForButton()
        {
            GameManager.m_instance.TogglePause();
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            ResetMenu();
        }

        public void ResetMenu()
        {
            _help.SetActive(false);
            _pauseBox.SetActive(true);
            _returnFromHelp.SetActive(false);
        }

        public void ResumeButton()
        {
            _gameManager.TogglePause();
        }

        public void HelpButton()
        {
            _help.SetActive(true);
            _pauseBox.SetActive(false);
            _returnFromHelp.SetActive(true);
        }

        public void MainMenuButton()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1.0f;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private GameObject _help;
        [SerializeField] private GameObject _pauseBox;
        [SerializeField] private GameObject _returnFromHelp;

        private GameManager _gameManager;

        #endregion
    }
}
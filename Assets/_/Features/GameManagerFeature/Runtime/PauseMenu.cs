using JetBrains.Annotations;
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

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            ResetMenu();
        }

        public void ResetMenu()
        {
            _help.SetActive(false);
            _pauseButtons.SetActive(true);
            _returnFromHelp.SetActive(false);
        }

        public void ResumeButton()
        {
            _gameManager.TogglePause();
        }

        public void HelpButton()
        {
            _help.SetActive(true);
            _pauseButtons.SetActive(false);
            _returnFromHelp.SetActive(true);
        }

        public void MainMenuButton()
        {
            SceneManager.LoadScene(0);
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private GameObject _help;
        [SerializeField] private GameObject _pauseButtons;
        [SerializeField] private GameObject _returnFromHelp;

        private GameManager _gameManager;

        #endregion
    }
}
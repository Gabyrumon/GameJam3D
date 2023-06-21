using ExternalOutline;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HUD.Runtime
{
    public class DivineInterventionHUD : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void LateUpdate()
        {
            if (_outline.enabled)
            {
                _descriptionGameObject.SetActive(true);
            }
            else
            {
                _descriptionGameObject.SetActive(false);
            }
        }

        #endregion

        #region Main Methods

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private Outline _outline;
        [SerializeField] private GameObject _descriptionGameObject;

        #endregion
    }
}
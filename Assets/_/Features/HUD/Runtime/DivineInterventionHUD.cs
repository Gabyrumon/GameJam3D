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
            if (_outline.enabled && !_descriptionGameObject.activeSelf)
            {
                _descriptionGameObject.SetActive(true);
                _description.color = _outline.OutlineColor;
            }
            else if (!_outline.enabled && _descriptionGameObject.activeSelf)
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
        [SerializeField] private TextMeshProUGUI _description;

        #endregion
    }
}
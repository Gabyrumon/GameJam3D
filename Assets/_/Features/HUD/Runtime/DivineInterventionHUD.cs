using ExternalOutline;
using TMPro;
using UnityEngine;
using Villager.Runtime;

namespace HUD.Runtime
{
    public class DivineInterventionHUD : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            _descriptionTMP.text = $"{_description}\n<wiggle>Cost: {_divineIntervention.OrbCost}</wiggle>";
        }

        private void LateUpdate()
        {
            if (_outline.enabled)
            {
                if (!_descriptionGameObject.activeSelf)
                {
                    _descriptionGameObject.SetActive(true);
                }
                _descriptionTMP.color = _outline.OutlineColor;
            }
            else if (!_outline.enabled && _descriptionGameObject.activeSelf)
            {
                _descriptionGameObject.SetActive(false);
            }
        }

        #endregion

        #region Private and Protected Members

        [TextArea]
        [SerializeField] private string _description;

        [SerializeField] private DivineIntervention _divineIntervention;
        [SerializeField] private Outline _outline;
        [SerializeField] private GameObject _descriptionGameObject;
        [SerializeField] private TextMeshProUGUI _descriptionTMP;

        #endregion
    }
}
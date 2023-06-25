using ChurchFeature.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD.Runtime
{
    public class FaithBarHUD : MonoBehaviour
    {
        #region Unity API
        
        private void OnEnable()
        {
            ChurchManager.Instance.m_onChurchStart += OnChurchStartEventHandler;
            ChurchManager.Instance.m_onFaithChanged += OnFaithChangedEventHandler;
        }

        private void OnDisable()
        {
            ChurchManager.Instance.m_onChurchStart -= OnChurchStartEventHandler;
            ChurchManager.Instance.m_onFaithChanged -= OnFaithChangedEventHandler;
        }

        #endregion

        #region Main Methods

        private void OnChurchStartEventHandler(object sender, OnChurchStartEventArgs e)
        {
            UpdateFillAmount(e.OnFaithChangedEventArgs.FaithCount, e.OnFaithChangedEventArgs.MaxFaithCount);
        }

        private void OnFaithChangedEventHandler(object sender, OnFaithChangedEventArgs e)
        {
            UpdateFillAmount(e.FaithCount, e.MaxFaithCount);
        }

        private void UpdateFillAmount(int faithCount, int maxFaithCount)
        {
            _faithFillerBar.fillAmount = (float)faithCount / maxFaithCount;
            _faithOrbText.text = $"{faithCount} / {maxFaithCount}";
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private TextMeshProUGUI _faithOrbText;
        [SerializeField] private Image _faithFillerBar;

        #endregion
    }
}
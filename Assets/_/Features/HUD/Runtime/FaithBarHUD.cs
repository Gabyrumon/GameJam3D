using System;
using ChurchFeature.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD.Runtime
{
    public class FaithBarHUD : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            _faithFillerBar.fillAmount = 0;
        }
        
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

        private void Update()
        {
            if (_deltaAmount > 1) return;
            
            _deltaAmount += Time.deltaTime / _fillSpeed;
            _faithFillerBar.fillAmount = Mathf.Lerp(_normalizedBaseFill, _normalizedTargetFill, _fillAnimationCurve.Evaluate(_deltaAmount));

            _faithOrbText.text = $"{Mathf.RoundToInt(_faithFillerBar.fillAmount * _currentMaxFaithCount)} / {_currentMaxFaithCount}";
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
            _normalizedBaseFill = _faithFillerBar.fillAmount;
            _normalizedTargetFill = (float)faithCount / maxFaithCount;
            _deltaAmount = 0;

            _currentMaxFaithCount = maxFaithCount;
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private TextMeshProUGUI _faithOrbText;
        [SerializeField] private Image _faithFillerBar;
        
        [Space]
        [Tooltip("In seconds")]
        [Range(0.01f, 2f)]
        [SerializeField] private float _fillSpeed = 1f;
        [SerializeField] private AnimationCurve _fillAnimationCurve;

        private float _normalizedBaseFill;
        private float _normalizedTargetFill;
        private float _deltaAmount;

        private float _currentMaxFaithCount;

        #endregion
    }
}
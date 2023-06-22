using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChurchFeature.Runtime
{
    public class Church : MonoBehaviour
    {
        #region Public Members

        public int m_level;

        public static Church m_instance;

        public int FaithOrbCount
        {
            get => _faithOrbCount;

            set
            {
                _faithOrbCount = value;
                if (_faithOrbCount < 0)
                {
                    _faithOrbCount = 0;
                }
                else if (_faithOrbCount > _maxFaithPerLevel[m_level])
                {
                    _faithOrbCount = _maxFaithPerLevel[m_level];
                }
                UpdateFillAmount();
                _judgmentHUD.SetActive(!IsJudgmentReady());
            }
        }

        public int JudgmentCost { get => _judgmentCost; set => _judgmentCost = value; }

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance != null) return;

            m_instance = this;
        }

        private void Start()
        {
            UpdateFillAmount();
            UpdateUpgradeCostText();
            _judgmentHUD.SetActive(!IsJudgmentReady());
        }

        private void Update()
        {
            if (CanUpgrade())
            {
                _upgradeButtonGameObject.SetActive(true);
            }
            else
            {
                _upgradeButtonGameObject.SetActive(false);
            }

            _levelDescriptionsParent.SetActive(IsMouseOverDescription());
        }

        private void OnGUI()
        {
            if (GUILayout.Button("UpgradeChurch"))
            {
                Upgrade();
            }
        }

        #endregion

        #region Main Methods

        private void UpdateFillAmount()
        {
            _faithFillerBar.fillAmount = (float)_faithOrbCount / _maxFaithPerLevel[m_level];
            _faithOrbText.text = $"{FaithOrbCount} / {_maxFaithPerLevel[m_level]}";
        }

        private void UpdateUpgradeCostText()
        {
            if (m_level < _upgradeCostPerLevel.Length)
            {
                _upgradeText.text = $"Upgrade: {_upgradeCostPerLevel[m_level]}";
            }
            else
            {
                _upgradeText.text = "";
            }
        }

        public void Upgrade()
        {
            if (!CanUpgrade()) return;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_levelsTransform.GetChild(m_level)
                .DOMoveY(-30, _deconstructAnimationDuration)
                .SetEase(_deconstructEase));

            sequence.Append(_levelsTransform.GetChild(m_level + 1)
                .DOMoveY(0, _buildAnimationDuration)
                .SetEase(_buildEase));

            sequence.Append(_levelsTransform.GetChild(m_level + 1)
                    .DOScaleY(_buildCompressionScale, _buildCompressionAnimationDuration)
                    .SetEase(_deconstructEase)
                    .SetLoops(2, LoopType.Yoyo));

            FaithOrbCount -= _upgradeCostPerLevel[m_level];

            m_level++;
            _levelDescriptionTexts[m_level].color = Color.white;
            UpdateFillAmount();
            _levelText.text = $"Church Level : {IntToRomanNotation(m_level + 1)}";
            UpdateUpgradeCostText();
            _judgmentHUD.SetActive(!IsJudgmentReady());
        }

        private string IntToRomanNotation(int number)
        {
            switch (number)
            {
                case 1:
                    return "I";

                case 2:
                    return "II";

                case 3:
                    return "III";

                case 4:
                    return "IV";
            }
            return "";
        }

        private bool CanUpgrade()
        {
            return m_level < _upgradeCostPerLevel.Length
                && FaithOrbCount >= _upgradeCostPerLevel[m_level];
        }

        public bool IsJudgmentReady()
        {
            return m_level >= _levelRequiredForJudgment && _faithOrbCount >= JudgmentCost;
        }

        private bool IsMouseOverDescription()
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(ScreenPosToPointerData(Input.mousePosition), results);
            foreach (var result in results)
            {
                if (result.gameObject == _levelText.gameObject)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Utils

        private PointerEventData ScreenPosToPointerData(Vector2 screenPos)
           => new(EventSystem.current) { position = screenPos };

        #endregion

        #region Private and Protected Members

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Transform _levelsTransform;
        [SerializeField] private GameObject _upgradeButtonGameObject;

        [Space]
        [SerializeField] private GameObject _levelDescriptionsParent;

        [SerializeField] private TextMeshProUGUI[] _levelDescriptionTexts;
        [SerializeField] private TextMeshProUGUI _upgradeText;

        [Space]
        [SerializeField] private int _faithOrbCount;

        [SerializeField] private TextMeshProUGUI _faithOrbText;

        [SerializeField] private int[] _maxFaithPerLevel;
        [SerializeField] private int[] _upgradeCostPerLevel;

        [SerializeField] private Image _faithFillerBar;

        [Space]
        [SerializeField] private int _judgmentCost;

        [SerializeField] private GameObject _judgmentHUD;
        [SerializeField] private int _levelRequiredForJudgment;

        [Space]
        [Header("Animations")]
        [Space]
        [SerializeField] private float _deconstructAnimationDuration;

        [SerializeField] private Ease _deconstructEase;

        [Space]
        [SerializeField] private float _buildAnimationDuration;

        [SerializeField] private Ease _buildEase;
        [SerializeField] private AnimationCurve _buildCompressionAnimation;
        [SerializeField] private float _buildCompressionAnimationDuration;

        [Range(0f, 1f)]
        [SerializeField] private float _buildCompressionScale;

        #endregion
    }
}
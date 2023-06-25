using System;
using System.Collections.Generic;
using Sound.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChurchFeature.Runtime
{
    public class OnChurchUpgradedEventArgs : EventArgs
    {
        public int LevelAfterUpgrade { get; set; }
    }
    
    public class Church : MonoBehaviour
    {
        #region Public Members

        public static Church Instance { get => _instance; private set => _instance = value; }

        public EventHandler<OnChurchUpgradedEventArgs> m_onUpgrade;

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
                else if (_faithOrbCount > _maxFaithPerLevel[Level])
                {
                    _faithOrbCount = _maxFaithPerLevel[Level];
                }

                UpdateFillAmount();
                _judgmentHUD.SetActive(!IsJudgmentReady());
            }
        }

        public int JudgmentCost { get => _judgmentCost; set => _judgmentCost = value; }

        public int Level { get => _level; private set => _level = value; }

        #endregion

        #region Unity API

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            UpdateFillAmount();
            UpdateUpgradeCostText();
            _judgmentHUD.SetActive(!IsJudgmentReady());
        }

        private void Update()
        {
            _upgradeButtonGameObject.SetActive(CanUpgrade());

            _levelDescriptionsParent.SetActive(IsMouseOverDescription());
        }

        #endregion

        #region Main Methods

        private void UpdateFillAmount()
        {
            _faithFillerBar.fillAmount = (float)_faithOrbCount / _maxFaithPerLevel[Level];
            _faithOrbText.text = $"{FaithOrbCount} / {_maxFaithPerLevel[Level]}";
        }

        private void UpdateUpgradeCostText()
        {
            if (Level < _upgradeCostPerLevel.Length)
            {
                _upgradeText.text = $"<incr a=2>{_upgradeCostPerLevel[Level]}</incr> <incr a=0.5>for Upgrade </incr>";
            }
            else
            {
                _upgradeText.text = "";
            }
        }

        public void Upgrade()
        {
            if (!CanUpgrade()) return;

            FaithOrbCount -= _upgradeCostPerLevel[Level];

            Level++;
            _levelDescriptionTexts[Level].color = Color.white;
            UpdateFillAmount();
            _levelText.text = $"Church Level : {IntToRomanNumbers(Level + 1)}";
            UpdateUpgradeCostText();
            _judgmentHUD.SetActive(!IsJudgmentReady());

            m_onUpgrade?.Invoke(this, new OnChurchUpgradedEventArgs() { LevelAfterUpgrade = Level} );
            SoundManager.m_instance.PlayChurchUpgrade();
        }

        private bool CanUpgrade()
        {
            return Level < _upgradeCostPerLevel.Length
                   && FaithOrbCount >= _upgradeCostPerLevel[Level];
        }

        public bool IsJudgmentReady()
        {
            return Level >= _levelRequiredForJudgment && _faithOrbCount >= JudgmentCost;
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

        private static string IntToRomanNumbers(int number)
        {
            return number switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                4 => "IV",
                _ => ""
            };
        }

        private static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };

        #endregion

        #region Private and Protected Members

        [SerializeField] private TextMeshProUGUI _levelText;
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

        private static Church _instance;

        private int _level;

        #endregion
    }
}
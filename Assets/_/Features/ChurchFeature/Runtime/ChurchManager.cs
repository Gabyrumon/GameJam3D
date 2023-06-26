using System;
using UnityEngine;

namespace ChurchFeature.Runtime
{
    public class OnChurchStartEventArgs : EventArgs
    {
        public OnChurchUpgradedEventArgs OnChurchUpgradedEventArgs { get; set; }
        public OnFaithChangedEventArgs OnFaithChangedEventArgs { get; set; }
    }
    
    public class OnChurchUpgradedEventArgs : EventArgs
    {
        public int LevelAfterUpgrade { get; set; }
        public int[] UpgradeCostPerLevel { get; set; }
    }
    
    public class OnFaithChangedEventArgs : EventArgs
    {
        public int FaithCount { get; set; }
        public int MaxFaithCount { get; set; }
    }
    
    public class ChurchManager : MonoBehaviour
    {
        #region Public Members

        public static ChurchManager Instance { get => _instance; private set => _instance = value; }

        public EventHandler<OnChurchStartEventArgs> m_onChurchStart;
        public EventHandler<OnChurchUpgradedEventArgs> m_onUpgrade;
        public EventHandler<OnFaithChangedEventArgs> m_onFaithChanged;

        public int FaithCount
        {
            get => _faithCount;
            set
            {
                _faithCount = value;
                if (_faithCount < 0)
                {
                    _faithCount = 0;
                }
                else if (_faithCount > _maxFaithPerLevel[Level])
                {
                    _faithCount = _maxFaithPerLevel[Level];
                }

                m_onFaithChanged?.Invoke(this, new OnFaithChangedEventArgs { FaithCount = FaithCount, MaxFaithCount = _maxFaithPerLevel[Level] });
            }
        }

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
            m_onChurchStart?.Invoke(this, new OnChurchStartEventArgs
            {
                OnChurchUpgradedEventArgs = new OnChurchUpgradedEventArgs { LevelAfterUpgrade = Level, UpgradeCostPerLevel = _upgradeCostPerLevel },
                OnFaithChangedEventArgs = new OnFaithChangedEventArgs { FaithCount = FaithCount, MaxFaithCount = _maxFaithPerLevel[Level] }
            });
        }

        #endregion

        #region Main Methods

        public void Upgrade()
        {
            if (!CanUpgrade()) return;

            Level++;

            FaithCount -= _upgradeCostPerLevel[Level - 1];
            m_onUpgrade?.Invoke(this, new OnChurchUpgradedEventArgs { LevelAfterUpgrade = Level, UpgradeCostPerLevel = _upgradeCostPerLevel } );
        }

        public bool CanUpgrade()
        {
            return Level < _upgradeCostPerLevel.Length
                   && FaithCount >= _upgradeCostPerLevel[Level];
        }

        #endregion

        #region Private and Protected Members
        
        [SerializeField] private int _faithCount;

        [Space]
        [SerializeField] private int[] _maxFaithPerLevel;
        [SerializeField] private int[] _upgradeCostPerLevel;

        private static ChurchManager _instance;

        private int _level;

        #endregion
    }
}
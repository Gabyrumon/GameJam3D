using JetBrains.Annotations;
using ChurchFeature.Runtime;
using Sound.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Villager.Runtime
{
    public class Prayer : MonoBehaviour
    {
        #region Public Members

        public bool IsPraying
        {
            get => _isPraying;
            set => _isPraying = value;
        }

        #endregion
        
        #region Unity API

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _churchManager = ChurchManager.Instance;
        }

        private void OnEnable()
        {
            StartPrayer();
        }

        private void Update()
        {
            _currentCooldown -= Time.deltaTime;
            _circleCooldown.fillAmount = _currentCooldown / _maxCooldown;
            if (_currentCooldown < 0)
            {
                FailPrayer();
            }
        }

        #endregion

        #region Main Methods

        [UsedImplicitly]
        public void StartPrayer()
        {
            if (IsPraying) return;
            
            _currentCooldown = _maxCooldown;
            _circleCooldown.fillAmount = _maxCooldown;
            gameObject.SetActive(true);
            
            IsPraying = true;
        }

        public void ValidatePrayer()
        {
            if (!IsPraying || SatanManager.m_instance._hasLaunchedGoWinTheGame) return;
            
            _churchManager.FaithCount += _orbGained;
            gameObject.SetActive(false);
            _villagerAI.StopPraying();
            SoundManager.m_instance.PlayVillagerVoiceJoy(_villagerAI.IsMan);
            
            IsPraying = false;
        }

        private void FailPrayer()
        {
            if (!IsPraying || SatanManager.m_instance._hasLaunchedGoWinTheGame) return;
            _villagerAI.IsConverted = false;

            _churchManager.FaithCount -= _orbLost;
            /*
            _frontImage.material.DOColor(Color.red, 0.5f)
                .OnComplete(() => gameObject.SetActive(false));
            */
            gameObject.SetActive(false);
            _villagerAI.StopPraying();
            SoundManager.m_instance.PlayVillagerVoiceSad(_villagerAI.IsMan);
            
            IsPraying = false;
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private VillagerAI _villagerAI;

        [SerializeField] private int _orbGained = 5;
        [SerializeField] private int _orbLost = 3;

        [SerializeField] private Image _circleCooldown;
        [SerializeField] private float _maxCooldown = 5;

        private float _currentCooldown;
        private ChurchManager _churchManager;
        private bool _isPraying;

        #endregion
    }
}
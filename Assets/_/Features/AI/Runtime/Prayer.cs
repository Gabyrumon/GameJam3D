using JetBrains.Annotations;
using ChurchFeature.Runtime;
using Sound.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Villager.Runtime
{
    public class Prayer : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _church = Church.Instance;
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
            if (_isPraying) return;
            
            _currentCooldown = _maxCooldown;
            _circleCooldown.fillAmount = _maxCooldown;
            gameObject.SetActive(true);
            
            _isPraying = true;
        }

        public void ValidatePrayer()
        {
            if (!_isPraying || SatanManager.m_instance._hasLaunchedGoWinTheGame) return;
            
            _church.FaithOrbCount += _orbGained;
            gameObject.SetActive(false);
            _villagerAI.StopPraying();
            SoundManager.m_instance.PlayVillagerVoiceJoy(_villagerAI.IsMan);
            
            _isPraying = false;
        }

        private void FailPrayer()
        {
            if (!_isPraying || SatanManager.m_instance._hasLaunchedGoWinTheGame) return;
            _villagerAI.IsConverted = false;

            _church.FaithOrbCount -= _orbLost;
            /*
            _frontImage.material.DOColor(Color.red, 0.5f)
                .OnComplete(() => gameObject.SetActive(false));
            */
            gameObject.SetActive(false);
            _villagerAI.StopPraying();
            SoundManager.m_instance.PlayVillagerVoiceSad(_villagerAI.IsMan);
            
            _isPraying = false;
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private VillagerAI _villagerAI;

        [SerializeField] private int _orbGained = 5;
        [SerializeField] private int _orbLost = 3;

        [SerializeField] private Image _circleCooldown;
        [SerializeField] private float _maxCooldown = 5;

        private float _currentCooldown;
        private Church _church;
        private bool _isPraying;

        #endregion
    }
}
using ChurchFeature.Runtime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Villager.Runtime
{
    public class Prayer : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Awake()
        {
            _villagerAI = GetComponent<VillagerAI>();
            _church = Church.m_instance;
            gameObject.SetActive(false);
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

        public void StartPrayer()
        {
            _currentCooldown = _maxCooldown;
            _circleCooldown.fillAmount = _maxCooldown;
            gameObject.SetActive(true);
        }

        public void ValidatePrayer()
        {
            Debug.Log("Prayer Validated");
            gameObject.SetActive(false);
            _church.m_faithOrbCount += _orbGained;
        }

        private void FailPrayer()
        {
            _villagerAI.m_isConverted = false;

            Debug.Log("Prayer Failed");
            /*
            _frontImage.material.DOColor(Color.red, 0.5f)
                .OnComplete(() => gameObject.SetActive(false));
            */
            gameObject.SetActive(false);
            _villagerAI.StopPraying();
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private VillagerAI _villagerAI;

        [SerializeField] private int _orbGained;

        [SerializeField] private Image _frontImage;
        [SerializeField] private Image _circleCooldown;
        [SerializeField] private float _maxCooldown;

        private float _currentCooldown;

        private Church _church;

        #endregion
    }
}
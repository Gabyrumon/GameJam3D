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
        }

        private void OnEnable()
        {
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

        public void StartPrayer(float coolDown)
        {
            _maxCooldown = coolDown;
            _currentCooldown = _maxCooldown;
            gameObject.SetActive(true);
        }

        public void ValidatePrayer()
        {
            gameObject.SetActive(true);
        }

        private void FailPrayer()
        {
            _frontImage.material.DOColor(Color.red, 0.5f)
                .OnComplete(() => gameObject.SetActive(false));
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private VillagerAI _villagerAI;

        [SerializeField] private Image _frontImage;
        [SerializeField] private Image _circleCooldown;
        [SerializeField] private float _maxCooldown;

        private float _currentCooldown;

        #endregion
    }
}
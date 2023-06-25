using ChurchFeature.Runtime;
using TMPro;
using UnityEngine;

namespace HUD.Runtime
{
    public class ChurchHUD : MonoBehaviour
    {
        #region Unity API
        
        private void OnEnable()
        {
            ChurchManager.Instance.m_onChurchStart += OnChurchStartEventHandler;
            ChurchManager.Instance.m_onUpgrade += OnUpgradeEventHandler;
            ChurchManager.Instance.m_onFaithChanged += OnFaithChangedEventHandler;
        }

        private void OnDisable()
        {
            ChurchManager.Instance.m_onChurchStart -= OnChurchStartEventHandler;
            ChurchManager.Instance.m_onUpgrade -= OnUpgradeEventHandler;
            ChurchManager.Instance.m_onFaithChanged -= OnFaithChangedEventHandler;
        }

        #endregion

        #region Main Methods

        private void OnChurchStartEventHandler(object sender, OnChurchStartEventArgs e)
        {
            UpdateUpgradeCostText(e.OnChurchUpgradedEventArgs.LevelAfterUpgrade, e.OnChurchUpgradedEventArgs.UpgradeCostPerLevel);
            UpgradeButtonSetActive();
        }
        
        private void OnUpgradeEventHandler(object sender, OnChurchUpgradedEventArgs e)
        {
            UpdateUpgradeCostText(e.LevelAfterUpgrade, e.UpgradeCostPerLevel);
        }

        private void UpdateUpgradeCostText(int levelAfterUpgrade, int[] upgradeCostPerLevel)
        {
            _upgradeText.text = levelAfterUpgrade < upgradeCostPerLevel.Length ?
                    $"<incr a=2>{upgradeCostPerLevel[levelAfterUpgrade]}</incr> <incr a=0.5>for Upgrade </incr>" :
                    "";
        }

        private void OnFaithChangedEventHandler(object sender, OnFaithChangedEventArgs e)
        {
            UpgradeButtonSetActive();
        }

        private void UpgradeButtonSetActive()
        {
            _upgradeButtonGameObject.SetActive(ChurchManager.Instance.CanUpgrade());
        }

        #endregion

        #region Private and Protected Members
        
        [SerializeField] private GameObject _upgradeButtonGameObject;
        [SerializeField] private TextMeshProUGUI _upgradeText;

        #endregion
    }
}
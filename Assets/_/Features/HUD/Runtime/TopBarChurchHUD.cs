using System.Collections.Generic;
using System.Linq;
using ChurchFeature.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Runtime;

namespace HUD.Runtime
{
    public class TopBarChurchHUD : MonoBehaviour
    {
        #region Unity API
        
        private void OnEnable()
        {
            ChurchManager.Instance.m_onChurchStart += OnChurchStartEventHandler;
            ChurchManager.Instance.m_onUpgrade += OnUpgradeEventHandler;
        }

        private void OnDisable()
        {
            ChurchManager.Instance.m_onChurchStart -= OnChurchStartEventHandler;
            ChurchManager.Instance.m_onUpgrade -= OnUpgradeEventHandler;
        }

        private void Update()
        {
            _levelDescriptionsParent.SetActive(IsMouseOverDescription());
        }

        #endregion

        #region Main Methods

        private void OnChurchStartEventHandler(object sender, OnChurchStartEventArgs e)
        {
            UpdateChurchLevelText(e.OnChurchUpgradedEventArgs.LevelAfterUpgrade);
        }
        
        private void OnUpgradeEventHandler(object sender, OnChurchUpgradedEventArgs e)
        {
            UpdateChurchLevelText(e.LevelAfterUpgrade);
            _levelDescriptionTexts[e.LevelAfterUpgrade].color = Color.white;
        }

        private void UpdateChurchLevelText(int levelAfterUpgrade)
        {
            _levelText.text = $"Church Level : {StringFormatting.IntToRomanNumbers(levelAfterUpgrade + 1)}";
        }

        private bool IsMouseOverDescription()
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(Mouse.ScreenPosToPointerData(Input.mousePosition), results);
            return results.Any(result => result.gameObject == _levelText.gameObject);
        }
        
        #endregion

        #region Private and Protected Members

        [SerializeField] private GameObject _levelDescriptionsParent;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI[] _levelDescriptionTexts;
        
        #endregion
    }
}
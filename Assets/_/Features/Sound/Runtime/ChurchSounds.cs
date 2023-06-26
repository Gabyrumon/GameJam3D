using ChurchFeature.Runtime;
using FMODUnity;
using UnityEngine;

namespace Sound.Runtime
{
    public class ChurchSounds : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            ChurchManager.Instance.m_onUpgrade += OnUpgrade;
        }

        private void OnDisable()
        {
            ChurchManager.Instance.m_onUpgrade -= OnUpgrade;
        }

        #endregion

        #region Main Methods

        private void OnUpgrade(object sender, OnChurchUpgradedEventArgs e)
        {
            PlayChurchUpgradeSound();
        }

        private void PlayChurchUpgradeSound()
        {
            RuntimeManager.PlayOneShot(_churchUpgrade);
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private EventReference _churchUpgrade;

        #endregion
    }
}
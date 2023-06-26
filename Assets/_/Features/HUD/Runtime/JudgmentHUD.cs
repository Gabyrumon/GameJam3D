using God.Runtime;
using UnityEngine;

namespace HUD.Runtime
{
    public class JudgmentHUD : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            GodServiceLocator.Instance.GodJudgment.m_onJudgmentReady += OnJudgmentReady;
        }

        private void OnDisable()
        {
            GodServiceLocator.Instance.GodJudgment.m_onJudgmentReady -= OnJudgmentReady;
        }

        #endregion
        
        #region Main Methods

        private void OnJudgmentReady(object sender, bool e)
        {
            TurnJudgmentIconOn(e);
        }

        private void TurnJudgmentIconOn(bool isActive)
        {
            _judgmentMask.SetActive(!isActive);
        }

        #endregion
        
        #region Private and Protected Members

        [SerializeField] private GameObject _judgmentMask;

        #endregion
    }
}
using DG.Tweening;
using UnityEngine;

namespace ChurchFeature.Runtime
{
    public class ChurchAnimation : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            ChurchManager.Instance.m_onUpgrade += OnChurchUpgradeEventHandler;
        }

        private void OnDisable()
        {
            ChurchManager.Instance.m_onUpgrade -= OnChurchUpgradeEventHandler;
        }

        #endregion
        
        #region Main Methods
        
        private void OnChurchUpgradeEventHandler(object sender, OnChurchUpgradedEventArgs e)
        {
            PlayUpgradeAnimation(e.LevelAfterUpgrade);
        }

        private void PlayUpgradeAnimation(int levelAfterUpgrade)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);

            sequence.Append(_levelsTransformParent.GetChild(levelAfterUpgrade - 1)
                .DOMoveY(-30, _deconstructAnimationDuration)
                .SetEase(_deconstructEase));

            sequence.Append(_levelsTransformParent.GetChild(levelAfterUpgrade)
                .DOMoveY(0, _buildAnimationDuration)
                .SetEase(_buildEase));

            sequence.Append(_levelsTransformParent.GetChild(levelAfterUpgrade)
                .DOScaleY(_buildCompressionScale, _buildCompressionAnimationDuration)
                .SetEase(_deconstructEase)
                .SetLoops(2, LoopType.Yoyo));
        }
        
        #endregion
        
        #region Private and Protected Members
        
        [SerializeField] private Transform _levelsTransformParent;
        
        [Space]
        [Header("Deconstruct")]
        [SerializeField] private float _deconstructAnimationDuration;
        [SerializeField] private Ease _deconstructEase;

        [Space]
        [Header("Build")]
        [SerializeField] private float _buildAnimationDuration;
        [SerializeField] private Ease _buildEase;
        [SerializeField] private float _buildCompressionAnimationDuration;
        [Range(0f, 1f)]
        [SerializeField] private float _buildCompressionScale;

        #endregion
    }
}
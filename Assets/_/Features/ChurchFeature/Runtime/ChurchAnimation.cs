using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChurchFeature.Runtime
{
    [RequireComponent(typeof(Church))]
    public class ChurchAnimation : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            _church.m_onUpgrade += PlayUpgradeAnimation;
        }

        private void OnDisable()
        {
            _church.m_onUpgrade -= PlayUpgradeAnimation;
        }

        #endregion
        
        #region Main Methods
        
        private void PlayUpgradeAnimation(object sender, OnChurchUpgradedEventArgs e)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.5f);

            sequence.Append(_levelsTransformParent.GetChild(e.LevelAfterUpgrade - 1)
                .DOMoveY(-30, _deconstructAnimationDuration)
                .SetEase(_deconstructEase));

            sequence.Append(_levelsTransformParent.GetChild(e.LevelAfterUpgrade)
                .DOMoveY(0, _buildAnimationDuration)
                .SetEase(_buildEase));

            sequence.Append(_levelsTransformParent.GetChild(e.LevelAfterUpgrade)
                .DOScaleY(_buildCompressionScale, _buildCompressionAnimationDuration)
                .SetEase(_deconstructEase)
                .SetLoops(2, LoopType.Yoyo));
        }
        
        #endregion
        
        #region Private and Protected Members
        
        [FormerlySerializedAs("_levelsTransform")] [SerializeField] private Transform _levelsTransformParent;
        
        [Space]
        [SerializeField] private float _deconstructAnimationDuration;
        [SerializeField] private Ease _deconstructEase;

        [Space]
        [SerializeField] private float _buildAnimationDuration;
        [SerializeField] private Ease _buildEase;
        [SerializeField] private float _buildCompressionAnimationDuration;

        [Range(0f, 1f)]
        [SerializeField] private float _buildCompressionScale;

        private Church _church;

        #endregion
    }
}
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChurchFeature.Runtime
{
    public class Church : MonoBehaviour
    {
        #region Public Members

        [HideInInspector] public int m_level;

        public static Church m_instance;

        public int m_faithOrbCount;

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance != null) return;

            m_instance = this;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("UpgradeChurch"))
            {
                Upgrade();
            }
        }

        #endregion

        #region Main Methods

        public void Upgrade()
        {
            if (m_faithOrbCount < _upgradeCostPerLevel[m_level] || m_level >= _upgradeCostPerLevel.Length - 1) return;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.GetChild(m_level)
                .DOMoveY(-30, _deconstructAnimationDuration)
                .SetEase(_deconstructEase));

            sequence.Append(transform.GetChild(m_level + 1)
                .DOMoveY(0, _buildAnimationDuration)
                .SetEase(_buildEase));

            sequence.Append(transform.GetChild(m_level + 1)
                    .DOScaleY(_buildCompressionScale, _buildCompressionAnimationDuration)
                    .SetEase(_deconstructEase)
                    .SetLoops(2, LoopType.Yoyo));

            m_level++;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [Space]
        [SerializeField] private int[] _upgradeCostPerLevel;

        [Space]
        [SerializeField] private float _deconstructAnimationDuration;

        [SerializeField] private Ease _deconstructEase;

        [Space]
        [SerializeField] private float _buildAnimationDuration;

        [SerializeField] private Ease _buildEase;
        [SerializeField] private AnimationCurve _buildCompressionAnimation;
        [SerializeField] private float _buildCompressionAnimationDuration;

        [Range(0f, 1f)]
        [SerializeField] private float _buildCompressionScale;

        #endregion
    }
}
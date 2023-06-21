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

        public int m_level;

        public static Church m_instance;

        public int FaithOrbCount
        {
            get => _faithOrbCount;

            set
            {
                _faithOrbCount = value;
                if (_faithOrbCount < 0)
                {
                    _faithOrbCount = 0;
                }
            }
        }

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance != null) return;

            m_instance = this;
        }

        private void Update()
        {
            if (CanUpgrade())
            {
                _upgradeButtonGameObject.SetActive(true);
            }
            else
            {
                _upgradeButtonGameObject.SetActive(false);
            }
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
            if (!CanUpgrade()) return;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_levelsTransform.GetChild(m_level)
                .DOMoveY(-30, _deconstructAnimationDuration)
                .SetEase(_deconstructEase));

            sequence.Append(_levelsTransform.GetChild(m_level + 1)
                .DOMoveY(0, _buildAnimationDuration)
                .SetEase(_buildEase));

            sequence.Append(_levelsTransform.GetChild(m_level + 1)
                    .DOScaleY(_buildCompressionScale, _buildCompressionAnimationDuration)
                    .SetEase(_deconstructEase)
                    .SetLoops(2, LoopType.Yoyo));

            FaithOrbCount -= _upgradeCostPerLevel[m_level];
            m_level++;
        }

        private bool CanUpgrade()
        {
            return m_level < _upgradeCostPerLevel.Length
                && FaithOrbCount >= _upgradeCostPerLevel[m_level];
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private Transform _levelsTransform;
        [SerializeField] private GameObject _upgradeButtonGameObject;

        [Space]
        [SerializeField] private int _faithOrbCount;

        [SerializeField] private int[] _upgradeCostPerLevel;

        [Space]
        [Header("Animations")]
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
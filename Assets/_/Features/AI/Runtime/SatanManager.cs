using System.Collections.Generic;
using UnityEngine;

namespace Villager.Runtime
{
    public class SatanManager : MonoBehaviour
    {

        #region Public Members

        public static SatanManager m_instance;

        #endregion


        #region Unity API

        private void Awake()
        {
            m_instance = this;
        }

        private void Start()
        {
            m_notPossessedVillagerList = m_villagerList;
            SetRandomTimeBeforePossession();
        }

        private void Update()
        {
            if (_timeBeforePossession > 0)
            {
                _timeBeforePossession -= Time.deltaTime;
                if (_timeBeforePossession <= 0)
                {
                    Possess();
                }
            }
        }

        #endregion


        #region Main Methods

        private void SetRandomTimeBeforePossession()
        {
            _timeBeforePossession = Random.Range(_randomTimeBeforePossession.x, _randomTimeBeforePossession.y);
        }

        private void Possess()
        {
            if (m_notPossessedVillagerList.Count > 0)
            {
                int randomIndex = Random.Range(0, m_notPossessedVillagerList.Count);

                m_villagerList[randomIndex].StartPossession();
                m_notPossessedVillagerList.RemoveAt(randomIndex);
            }

            SetRandomTimeBeforePossession();
        }

        #endregion


        #region Utils

        #endregion


        #region Private and Protected Members

        public List<DarkSideAI> m_villagerList = new List<DarkSideAI>();

        public List<DarkSideAI> m_notPossessedVillagerList = new List<DarkSideAI>();

        [Tooltip("In seconds")]
        [SerializeField] private Vector2 _randomTimeBeforePossession = new Vector2(30,60);
        private float _timeBeforePossession;

        #endregion
    }
}
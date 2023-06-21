using System.Collections.Generic;
using UnityEngine;

namespace Villager.Runtime
{
    public class SatanManager : MonoBehaviour
    {
        #region Public Members

        public static SatanManager m_instance;

        public List<DarkSideAI> m_villagerList = new List<DarkSideAI>();

        public List<DarkSideAI> m_notPossessedVillagerList = new List<DarkSideAI>();

        public List<DemonAI> m_demonList = new List<DemonAI>();

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
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
                DarkSideAI current = m_notPossessedVillagerList[randomIndex];

                for (int i = 0; i < m_notPossessedVillagerList.Count; i++)
                {
                    if (m_notPossessedVillagerList[(randomIndex + i) % m_notPossessedVillagerList.Count].GetComponent<VillagerAI>().CurrentState != VillagerAI.VillagerState.Pray)
                    {
                        current.StartPossession();
                        m_notPossessedVillagerList.RemoveAt(randomIndex);
                        break;
                    }
                }
            }

            SetRandomTimeBeforePossession();
        }

        public void SpawnDemon(DemonAI demonAI)
        {
            m_demonList.Add(demonAI);

            for (int i = 0; i < m_villagerList.Count; i++)
            {
                m_villagerList[i].GetComponent<VillagerAI>().ChangeState(VillagerAI.VillagerState.Afraid);
            }
        }

        public void DemonIsKill(DemonAI demonAI)
        {
            m_demonList.Remove(demonAI);

            if (m_demonList.Count == 0)
            {
                for (int i = 0; i < m_villagerList.Count; i++)
                {
                    m_villagerList[i].GetComponent<VillagerAI>().ReturnToRoutine();
                }
            }
        }

        #endregion

        #region Utils

        #endregion


        #region Private And Protected Members

        [Tooltip("In seconds")]
        [SerializeField] private Vector2 _randomTimeBeforePossession = new Vector2(30, 60);
        private float _timeBeforePossession;

        #endregion
    }
}
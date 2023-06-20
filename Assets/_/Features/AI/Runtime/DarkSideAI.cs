using UnityEngine;
using static Villager.Runtime.VillagerAI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(VillagerAI))]
    public class DarkSideAI : MonoBehaviour
    {
        #region Public Memebers

        public bool m_isPossessed;

        #endregion


        #region Unity API

        private void OnEnable()
        {
            SatanManager.m_instance.m_villagerList.Add(this);
        }

        private void OnDisable()
        {
            SatanManager.m_instance.m_villagerList.Remove(this);
        }

        private void Start()
        {
            _villagerAI = GetComponent<VillagerAI>();
        }

        private void Update()
        {
            if (_timeBeforeNextPossession > 0)
            {
                _timeBeforeNextPossession -= Time.deltaTime;
                if (_timeBeforeNextPossession <= 0)
                {
                    Possess();
                    SetRandomTimeBeforePossession();
                }
            }
        }

        #endregion


        #region Main Methods

        public void StartPossession()
        {
            m_isPossessed = true;

            SetRandomTimeBeforePossession();
            Possess();
        }

        public void ResetPossession()
        {
            m_isPossessed = false;
            _levelOfPossession = 0;
        }

        private void Possess()
        {
            if (_levelOfPossession == 0)
            {
                _villagerAI.ChangeState(VillagerState.Steal);
            }
            else if (_levelOfPossession == 1)
            {
                _villagerAI.ChangeState(VillagerState.Kill);
            }
            else if (_levelOfPossession == 2)
            {
                _villagerAI.ChangeState(VillagerState.Ritual);
            }

            _levelOfPossession++;
        }

        private void SetRandomTimeBeforePossession()
        {
            _timeBeforeNextPossession = Random.Range(_randomTimeBetweenPosssessions.x, _randomTimeBetweenPosssessions.y);
        }

        #endregion


        #region Private And Protected Members

        [SerializeField] private Vector2 _randomTimeBetweenPosssessions;

        private float _timeBeforeNextPossession;
        private int _levelOfPossession = 0;
        private VillagerAI _villagerAI;

        #endregion
    }
}
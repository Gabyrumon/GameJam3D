using UnityEngine;
using static Villager.Runtime.VillagerAI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(VillagerAI))]
    public class DarkSideAI : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void OnEnable()
        {
            SatanManager.m_instance.m_notPossessedVillagerList.Add(this);
        }

        private void OnDisable()
        {
            SatanManager.m_instance.m_notPossessedVillagerList.Remove(this);
        }

        private void Start()
        {
            _villagerAI = GetComponent<VillagerAI>();
        }

        private void Update()
        {
            if (!_isPossessed) return;

            if (_timeBeforeNextPossession > 0 && _villagerAI.CurrentState == VillagerState.Routine)
            {
                _timeBeforeNextPossession -= Time.deltaTime;
                if (_timeBeforeNextPossession <= 0)
                {
                    Possess();
                    SetRandomTimeBeforePossession();
                }
            }
        }

        private void OnGUI()
        {
            //if (GUILayout.Button("Hit")) GetHit();
        }

        #endregion

        #region Main Methods

        private void ResetPossession()
        {
            _isPossessed = false;
            _levelOfPossession = 0;
            SatanManager.m_instance.m_notPossessedVillagerList.Add(this);
        }

        private void MinusPossession()
        {
            _isPossessed = false;
            _levelOfPossession--;
            SatanManager.m_instance.m_notPossessedVillagerList.Add(this);
        }

        public void StartPossession()
        {
            _isPossessed = true;

            SetRandomTimeBeforePossession();
            Possess();
        }

        public void GetHit()
        {
            if (_isPossessed)
            {
                ResetPossession();
                //MinusPossession();

                _villagerAI.ReturnToRoutine();
            }
            else
            {
                _villagerAI.IsConverted = false;
            }

            _villagerAI.HitAnim();
        }

        private void Possess()
        {
            if (_villagerAI.GetState() == VillagerState.Pray)
            {
                ResetPossession();
                return;
            }

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

        private VillagerAI _villagerAI;
        private float _timeBeforeNextPossession;
        private int _levelOfPossession = 0;
        private bool _isPossessed;

        #endregion
    }
}
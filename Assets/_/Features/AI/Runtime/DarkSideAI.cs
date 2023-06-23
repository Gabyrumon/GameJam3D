using Sound.Runtime;
using UnityEngine;
using static Villager.Runtime.VillagerAI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(VillagerAI))]
    public class DarkSideAI : MonoBehaviour
    {
        #region Public Members

        public bool IsPossessed { get => _isPossessed; set => _isPossessed = value; }

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

        public void ResetPossession(bool addFromNotPossessedList = true)
        {
            _isPossessed = false;
            _levelOfPossession = 0;
            _timeBeforeNextPossession = 0;
            if (!addFromNotPossessedList) return;

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
            IsPossessed = true;

            SetRandomTimeBeforePossession();
            Possess();
        }

        public void GetHit()
        {
            if (IsPossessed)
            {
                ResetPossession();
                //MinusPossession();
                SoundManager.m_instance.PlayVillagerVoiceJoy(_villagerAI.IsMan);

                _villagerAI.ReturnToRoutine();
            }
            else
            {
                SoundManager.m_instance.PlayVillagerVoiceHit(_villagerAI.IsMan);
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

            SoundManager.m_instance.PlayVillagerVoiceGrowl(_villagerAI.IsMan);
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
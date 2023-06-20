using Locator.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class VillagerAI : MonoBehaviour
    {
        #region Public Members

        [HideInInspector] public bool m_isConverted;

        #endregion

        #region Unity API

        private void Start()
        {
            _currentState = VillagerState.Routine;
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();

            _currentLocator = LocatorSystem.GetNearestLocation(_currentRoom, transform.position);
            _agent.SetDestination(_currentLocator.transform.position);

            _timeBeforePray = Random.Range(_randomTimeToPray.x, _randomTimeToPray.y);
            _agent.speed = _speed;
        }

        private void Update()
        {
            if (_timeBeforePray > 0 && _currentState == VillagerState.Routine && m_isConverted)
            {
                _timeBeforePray -= Time.deltaTime;
                if (_timeBeforePray <= 0)
                {
                    ChangeState(VillagerState.Pray);
                }
            }

            switch (_currentState)
            {
                case VillagerState.Surprise:
                    DoSurprise();
                    break;

                case VillagerState.Routine:
                    DoRoutine();
                    break;

                case VillagerState.Pray:
                    GoToChurch();
                    break;

                case VillagerState.Steal:
                    GoToMarket();
                    break;

                case VillagerState.Kill:
                    GoToKill();
                    break;

                case VillagerState.Ritual:
                    GoToRitual();
                    break;

                case VillagerState.Dead:
                    DoDeath();
                    break;

                default:
                    break;
            }
        }

        //private void OnGUI()
        //{
        //    //if (GUILayout.Button("Pray")) { _currentState = VillagerState.Pray; _actionPlayed = false; }

        //    if (GUILayout.Button("Steal")) ChangeState(VillagerState.Steal);

        //    if (GUILayout.Button("Kill")) ChangeState(VillagerState.Kill);

        //    if (GUILayout.Button("Ritual")) ChangeState(VillagerState.Ritual);

        //    if (GUILayout.Button("Surprise")) { _currentState = VillagerState.Surprise; _actionPlayed = false; }

        //    if (GUILayout.Button("Death")) { _currentState = VillagerState.Dead; _actionPlayed = false; }
        //}

        #endregion

        #region Main Methods

        private void DoRoutine()
        {
            if (!_actionPlayed)
            {
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 0.5f)
            {
                if (_currentLocator == LocatorSystem.m_locatorDict[_currentRoom][LocatorSystem.m_locatorDict[_currentRoom].Count - 1])
                {
                    _rePath = true;
                }
                else if (_currentLocator == LocatorSystem.m_locatorDict[_currentRoom][0])
                {
                    _rePath = false;
                }

                if (!_rePath)
                {
                    _currentLocator = LocatorSystem.GetNextLocation(_currentRoom, _currentLocator);
                }
                else
                {
                    _currentLocator = LocatorSystem.GetPreviousLocation(_currentRoom, _currentLocator);
                }

                _agent.SetDestination(_currentLocator.transform.position);
            }
        }

        private void GoToChurch()
        {
            if (!_actionPlayed)
            {
                _agent.SetDestination(LocatorSystem.GetNearestLocation(Room.Church, transform.position).transform.position);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 1.5f)
            {
                StartPraying();
            }
        }

        private void GoToMarket()
        {
            if (!_actionPlayed)
            {
                _agent.SetDestination(LocatorSystem.GetNearestLocation(Room.Market, transform.position).transform.position);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 1.5f)
            {
                StartStealing();
            }
        }

        private void GoToKill()
        {
            if (!_actionPlayed)
            {
                _agent.SetDestination(LocatorSystem.GetNearestLocation(Room.Enclosure, transform.position).transform.position);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 1.5f)
            {
                StartKilling();
            }
        }

        private void GoToRitual()
        {
            if (!_actionPlayed)
            {
                _agent.SetDestination(LocatorSystem.GetNearestLocation(Room.Ritual, transform.position).transform.position);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 1.5f)
            {
                StartRitual();
            }
        }

        #endregion

        #region Utils

        public void SetConvert(bool isConverted = true)
        {
            m_isConverted = isConverted;
        }

        private void StartPraying()
        {
            _anim.SetBool("Walk", false);
            _anim.SetBool("Pray", true);
            _agent.isStopped = true;

            _prayer.StartPrayer();
        }

        public void StopPraying()
        {
            _anim.SetBool("Pray", false);
            _agent.isStopped = false;

            _timeBeforePray = Random.Range(_randomTimeToPray.x, _randomTimeToPray.y);
        }

        private void StartStealing()
        {
            _anim.SetBool("Walk", false);
            _anim.SetBool("Steal", true);
            _agent.isStopped = true;
        }

        public void StopStealing()
        {
            _anim.SetBool("Steal", false);
            _agent.isStopped = false;

            ChangeState(VillagerState.Routine);
        }

        private void StartKilling()
        {
            _anim.SetBool("Walk", false);
            _anim.SetBool("Kill", true);
            _agent.isStopped = true;
        }

        public void StopKilling()
        {
            _anim.SetBool("Kill", false);
            _agent.isStopped = false;

            ChangeState(VillagerState.Routine);
        }

        private void StartRitual()
        {
            _anim.SetBool("Walk", false);
            _anim.SetBool("Ritual", true);
            _agent.isStopped = true;
        }

        public void StopRitual()
        {
            _anim.SetBool("Ritual", false);
            _agent.isStopped = false;

            ChangeState(VillagerState.Routine);
        }

        public void ChangeState(VillagerState state)
        {
            _agent.isStopped = true;
            _actionPlayed = false;
            _agent.isStopped = false;
            _currentState = state;
        }

        private void DoSurprise()
        {
            if (!_actionPlayed)
            {
                _agent.isStopped = true;
                _anim.SetTrigger("Surprise");

                _actionPlayed = true;
            }
        }

        private void DoDeath()
        {
            if (!_actionPlayed)
            {
                _agent.isStopped = true;
                _anim.SetTrigger("Death");

                _actionPlayed = true;
            }
        }

        #endregion

        #region Private And Protected Members

        public enum VillagerState
        {
            Routine,
            Pray,

            Steal,
            Kill,
            Ritual,

            Surprise,
            Dead
        }

        private enum OrderType
        {
            WalkTo,
            PickUp,

            Sacrifice,
            Resurection,
        }

        [SerializeField] private Room _currentRoom;
        [SerializeField] private VillagerState _currentState;

        [Space]
        [SerializeField] private float _speed;

        [Space]
        [Tooltip("In seconds")]
        [SerializeField] private Vector2 _randomTimeToPray = new Vector2(15, 45);

        [Space]
        [SerializeField] private Prayer _prayer;

        private float _timeBeforePray;

        private LocatorIdentity _currentLocator;
        private bool _rePath;
        private NavMeshAgent _agent;
        private Animator _anim;
        private bool _actionPlayed;

        #endregion
    }
}
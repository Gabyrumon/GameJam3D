using Interaction.Runtime;
using Locator.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class VillagerAI : MonoBehaviour
    {
        #region Public Members

        public bool m_isConverted;

        public bool m_isSelected;
        public DivineIntervention m_divineIntervention;

        public VillagerState CurrentState
        { get => _currentState; set { _currentState = value; } }

        #endregion

        #region Unity API

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _agent.speed = _speed;
            _timeBeforePray = Random.Range(_randomTimeBeforePraying.x, _randomTimeBeforePraying.y);
            _currentLocator = LocatorSystem.GetNearestLocation(_currentRoom, transform.position);

            _agent.SetDestination(_currentLocator.transform.position);
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
                case VillagerState.Routine:
                    DoRoutine();
                    break;

                case VillagerState.Pray:
                    GoTo(Room.Church, "Pray");
                    break;

                case VillagerState.BarrelAction:
                    GoTo(Room.Barrel, "BarrelAction");
                    break;

                case VillagerState.Steal:
                    GoTo(Room.Market, "Steal");
                    break;

                case VillagerState.Kill:
                    GoTo(Room.Enclosure, "Kill");
                    break;

                case VillagerState.Ritual:
                    GoTo(Room.Ritual, "Ritual");
                    break;

                case VillagerState.Dead:
                    DoDeath();
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Main Methods

        private void DoRoutine()
        {
            if (!_actionPlayed)
            {
                _anim.SetBool("Pray", false);

                _anim.SetBool("Steal", false);
                _anim.SetBool("Kill", false);
                _anim.SetBool("Ritual", false);

                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 1.5f)
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

        private void GoTo(Room roomToGo, string animName)
        {
            if (!_actionPlayed)
            {
                _agent.SetDestination(LocatorSystem.GetNearestLocation(roomToGo, transform.position).transform.position);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 1.5f && !_animPlayed)
            {
                _animPlayed = true;
                StartAnim(animName);
            }
        }

        #endregion

        #region Utils

        public void SetConvert(bool isConverted = true)
        {
            m_isConverted = isConverted;
        }

        public void SetTimeBeforeNextPray()
        {
            _timeBeforePray = Random.Range(_randomTimeBeforePraying.x, _randomTimeBeforePraying.y);
        }

        public void StopPraying()
        {
            SetTimeBeforeNextPray();

            _anim.SetBool("Pray", false);
        }

        public void ReturnToRoutine()
        {
            if (_currentState == VillagerState.Dead) return;

            ChangeState(VillagerState.Routine);
        }

        public void ChangeState(VillagerState state)
        {
            _agent.isStopped = true;
            _actionPlayed = false;
            _animPlayed = false;
            _agent.isStopped = false;
            _currentState = state;
        }

        public void HitAnim()
        {
            _anim.SetTrigger("Hit");
        }

        public void ActivateDivineIntervention()
        {
            m_divineIntervention.Interact();
        }

        public void DoDeath()
        {
            if (!_actionPlayed)
            {
                _anim.SetTrigger("Death");
                _anim.SetLayerWeight(1, 0.1f);
                _agent.isStopped = true;
                _actionPlayed = true;
            }
        }

        public VillagerState GetState() => _currentState;

        private void StartAnim(string animName)
        {
            if (animName.Equals("Pray"))
            {
                _prayerInteraction.SetActive(true);
            }
            else if (animName.Equals("BarrelAction"))
            {
                _prayerInteraction.SetActive(true);
            }

            _anim.SetBool("Walk", false);
            _anim.SetBool(animName, true);
            _agent.isStopped = true;
        }

        #endregion

        #region Private And Protected Members

        public enum VillagerState
        {
            Routine,
            Pray,
            Surprise,

            BarrelAction,

            Steal,
            Kill,
            Ritual,

            Dead
        }

        [SerializeField] private Room _currentRoom;

        [Space]
        [Tooltip("Meters per seconds")]
        [SerializeField] private float _speed;

        [Tooltip("In seconds")]
        [SerializeField] private Vector2 _randomTimeBeforePraying = new Vector2(15, 45);

        [Space]
        [SerializeField] private GameObject _prayerInteraction;

        private VillagerState _currentState;
        private LocatorIdentity _currentLocator;
        private NavMeshAgent _agent;
        private Animator _anim;
        private float _timeBeforePray;
        private bool _actionPlayed;
        private bool _animPlayed;
        private bool _rePath;

        #endregion
    }
}
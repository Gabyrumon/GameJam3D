using Locator.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class VillagerAI : MonoBehaviour
    {
        #region Public Members

        public DivineIntervention m_divineIntervention;

        public VillagerState CurrentState
        { get => _currentState; set { _currentState = value; } }

        public bool IsConverted { get => _isConverted; set { _isConverted = value; FaithVFX(); } }

        #endregion

        #region Unity API

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();
        }

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
            _agent.speed = _speed;
            _timeBeforePray = Random.Range(_randomTimeBeforePraying.x, _randomTimeBeforePraying.y);
            _currentLocator = LocatorSystem.GetNearestLocation(_currentRoom, transform.position);

            _agent.SetDestination(_currentLocator.transform.position);
            FaithVFX();
        }

        private void Update()
        {
            if (_timeBeforePray > 0 && _currentState == VillagerState.Routine && IsConverted)
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

                case VillagerState.Idle:
                    DoIdle(false);
                    break;
                
                case VillagerState.Afraid:
                    DoIdle(true);
                    break;

                case VillagerState.BarrelAction:
                    GoTo(m_divineIntervention, "Barrel");
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

        private void OnGUI()
        {
            if (GUILayout.Button("Faith")) IsConverted = true;
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

                _anim.SetBool("Surprise", false);

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

        private void GoTo(DivineIntervention interventionToGo, string animName)
        {
            if (!_actionPlayed)
            {
                _agent.SetDestination(interventionToGo.transform.position);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (_agent.remainingDistance < 2f && !_animPlayed)
            {
                _animPlayed = true;
                StartAnim("BarrelAction");
            }
        }

        #endregion

        #region Utils

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

        public void DoIdle(bool isSurprised)
        {
            if (!_actionPlayed)
            {
                _anim.SetBool("Pray", false);
                _anim.SetBool("Steal", false);
                _anim.SetBool("Kill", false);
                _anim.SetBool("Ritual", false);
                _anim.SetBool("Walk", false);
                _anim.SetBool("Surprise", false);
                _actionPlayed = true;

                _agent.isStopped = true;

                if (isSurprised)
                {
                    _anim.SetBool("Surprise", true);
                }
            }
        }

        public void DoDeath()
        {
            if (!_actionPlayed)
            {
                SatanManager.m_instance.m_villagerList.Remove(this);
                IsConverted = false;
                _anim.SetTrigger("Death");
                _anim.SetLayerWeight(1, 0.1f);
                _agent.isStopped = true;
                _actionPlayed = true;
                _agent.enabled = false;
            }
        }

        private void FaithVFX()
        {
            if (_isConverted)
            {
                _faithVFX.SetActive(true);
            }
            else
            {
                _faithVFX.SetActive(false);
            }
        }

        public void LaunchInvocationVFX()
        {
            Instantiate(_invocationVFX, transform.position, Quaternion.Euler(Vector3.right * 90f));
        }

        public void InvokeDemon()
        {
            Instantiate(_demonPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public VillagerState GetState() => _currentState;

        private void StartAnim(string animName)
        {
            if (animName.Equals("Pray"))
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
            Idle,
            Afraid,

            Steal,
            Kill,
            Ritual,

            Dead
        }

        [SerializeField] private Room _currentRoom;
        [SerializeField] private VillagerState _currentState;

        [SerializeField] private bool _isConverted;

        [Space]
        [Tooltip("Meters per seconds")]
        [SerializeField] private float _speed;

        [Tooltip("In seconds")]
        [SerializeField] private Vector2 _randomTimeBeforePraying = new Vector2(15, 45);

        [Space]
        [SerializeField] private GameObject _prayerInteraction;
        [SerializeField] private GameObject _demonPrefab;


        [Header("VFX")]
        [SerializeField] private GameObject _faithVFX;
        [SerializeField] private GameObject _invocationVFX;

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
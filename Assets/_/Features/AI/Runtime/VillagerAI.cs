using JetBrains.Annotations;
using Locator.Runtime;
using Sound.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Villager.Runtime
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class VillagerAI : MonoBehaviour
    {
        #region Public Members

        public VillagerState CurrentState { get => _currentState; set => _currentState = value; }

        public bool IsConverted
        {
            get => _isConverted;
            set
            {
                if (!_isConverted && value) SoundManager.m_instance.PlayVillagerVoiceSurprised(IsMan);
                _isConverted = value;
                FaithVFX();
            }
        }

        public bool IsMan { get => _isMan; set => _isMan = value; }
        public float TimeBeforePray { get => _timeBeforePray; set => _timeBeforePray = value; }
        public DivineIntervention DivineIntervention { get => _divineIntervention; set => _divineIntervention = value; }

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
                    GoTo(DivineIntervention, "BarrelAction");
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

                case VillagerState.GoToChurch:
                    GoTo(Room.WinChuch, "Surprise");
                    _agent.speed = 10f;
                    _anim.speed = 5f;
                    break;

                case VillagerState.Dead:
                    Die();
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

                _anim.SetBool("Surprise", false);

                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (!(_agent.remainingDistance < 1.5f)) return;
            
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

        private void GoTo(Room roomToGo, string animName)
        {
            if (!_actionPlayed)
            {
                _target = LocatorSystem.GetNearestLocation(roomToGo, transform.position).transform.position;
                _agent.SetDestination(_target);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (!(Vector3.Distance(transform.position, _target) < 1.5f) || _animPlayed) return;
            
            switch (animName)
            {
                case "Ritual":
                    SoundManager.m_instance.PlayDemonSpawnedHornSound();
                    break;
                case "Steal":
                    SoundManager.m_instance.PlayVillagerVoiceSlyLaugh(IsMan);
                    break;
            }

            _animPlayed = true;
            StartAnim(animName);
        }

        private void GoTo(DivineIntervention interventionToGo, string animName)
        {
            if (!_actionPlayed)
            {
                var interventionPosition = interventionToGo.transform.position;
                _target = interventionPosition;
                _agent.SetDestination(interventionPosition);
                _anim.SetBool("Walk", true);
                _actionPlayed = true;
            }

            if (!(Vector3.Distance(transform.position, _target) < 2f) || _animPlayed) return;
            
            _anim.speed = 1f;
            _animPlayed = true;
            StartAnim(animName);
        }

        #endregion

        #region Utils

        [UsedImplicitly]
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
            ChangeState(VillagerState.Routine);
        }

        public void ChangeState(VillagerState state)
        {
            if (_currentState is VillagerState.Pray) _prayer.ValidatePrayer();
            
            if (_currentState is VillagerState.Dead or VillagerState.GoToChurch) return;
            _actionPlayed = false;
            _animPlayed = false;

            _agent.isStopped = false;
            _currentState = state;
        }

        public void HitAnim()
        {
            _anim.SetTrigger("Hit");

            if (CurrentState == VillagerState.Routine) return;

            ReturnToRoutine();
        }

        [UsedImplicitly]
        public void ActivateDivineIntervention()
        {
            DivineIntervention.Interact(this);
        }

        [UsedImplicitly]
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

        [UsedImplicitly]
        public void Die()
        {
            if (_actionPlayed) return;
            
            GetComponent<DarkSideAI>().ResetPossession(false);
            ChangeState(VillagerState.Dead);
            SatanManager.m_instance.m_villagerList.Remove(this);

            SatanManager.m_instance.m_notPossessedVillagerList.Remove(GetComponent<DarkSideAI>());
            IsConverted = false;
            _anim.SetTrigger("Death");
            _anim.SetLayerWeight(1, 0.1f);
            _actionPlayed = true;
            _agent.enabled = false;
        }

        private void FaithVFX()
        {
            if (_isConverted)
            {
                _faithVFX.SetActive(true);
                SatanManager.m_instance.m_villagerHasFaithList.Add(this);
            }
            else
            {
                if (!_faithVFX.activeSelf) return;

                _faithVFX.SetActive(false);
                SatanManager.m_instance.m_villagerHasFaithList.Remove(this);
                if (SatanManager.m_instance.m_villagerHasFaithList.Count <= 0)
                {
                    SatanManager.m_instance.StartInvokingAllDemons();
                }
            }
        }

        public void LaunchInvocationVFX()
        {
            Debug.Log("VFX");
            Instantiate(_invocationVFX, transform.position, Quaternion.Euler(Vector3.right * 90f));
        }

        public void InvokeDemon()
        {
            Instantiate(_demonPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        [UsedImplicitly]
        public void PlayKillSound()
        {
            SoundManager.m_instance.PlaySheepDeath();
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

            GoToChurch,
            Dead
        }

        [SerializeField] private Room _currentRoom;
        [SerializeField] private VillagerState _currentState;

        [SerializeField] private bool _isConverted;

        [SerializeField] private bool _isMan;

        [Space]
        [Tooltip("Meters per seconds")]
        [SerializeField] private float _speed;
        [SerializeField] private Prayer _prayer;

        [Tooltip("In seconds")]
        [SerializeField] private Vector2 _randomTimeBeforePraying = new Vector2(15, 45);

        [Space]
        [SerializeField] private GameObject _prayerInteraction;

        [SerializeField] private GameObject _demonPrefab;

        [Header("VFX")]
        [SerializeField] private GameObject _faithVFX;

        [SerializeField] private GameObject _invocationVFX;

        private LocatorIdentity _currentLocator;
        private Vector3 _target;
        private NavMeshAgent _agent;
        private Animator _anim;
        private float _timeBeforePray;
        private bool _actionPlayed;
        private bool _animPlayed;
        private bool _rePath;

        private DivineIntervention _divineIntervention;

        #endregion
    }
}
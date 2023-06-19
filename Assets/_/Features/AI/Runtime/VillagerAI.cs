using Locator.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Villager.Runtime
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class VillagerAI : MonoBehaviour
    {

        #region Unity API

        private void Start()
        {
            _currentState = VillagerState.Routine;
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();

            _currentLocator = LocatorSystem.GetNearestLocation(_currentRoom, transform.position);
            _agent.SetDestination(_currentLocator.transform.position);
        }

        private void Update()
        {

            switch (_currentState)
            {
                case VillagerState.Routine:
                    DoRoutine();
                    break;

                case VillagerState.Surprise:
                    DoSurprise();
                    break;

                case VillagerState.Order:
                    
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
            if (GUILayout.Button("Surprise")) { _currentState = VillagerState.Surprise;_actionPlayed = false; }

            if (GUILayout.Button("Death")) { _currentState = VillagerState.Dead; _actionPlayed = false; }
        }

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

        private void DoSurprise()
        {
            if (!_actionPlayed)
            {
                _agent.isStopped = true;
                _anim.SetTrigger("Surprise");

                _actionPlayed = true;
            }

        }

        public void GoRoutine()
        {
            _actionPlayed = false;
            _agent.isStopped = false;
            _currentState = VillagerState.Routine;
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


        #region Utils

        #endregion


        #region Private And Protected Members

        private enum VillagerState
        {
            Routine,
            Surprise,
            Flee,
            Order,
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

        private LocatorIdentity _currentLocator;
        bool _rePath;
        private NavMeshAgent _agent;
        private Animator _anim;
        private bool _actionPlayed;

        #endregion
    }
}
using Locator.Runtime;
using UnityEditor;
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
            _currentState = EnemyState.Patrol;
            _agent = GetComponent<NavMeshAgent>();

            _radiusWarningSquared = _radiusWarning * _radiusWarning;
            _radiusCombatSquared = _radiusCombat * _radiusCombat;

            _currentLocator = LocatorSystem.GetFirstLocationPriority(_currentRoom);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Patrol:
                    Patrolling();
                    break;
                
                case EnemyState.Warning:
                    Warning();
                    break;
                
                case EnemyState.Combat:
                    Combat();
                    break;

                default:
                    break;
            }
        }
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position,Vector3.up, _radiusCombat);

            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position,Vector3.up, _radiusWarning);
        }

        #endregion


        #region Main Methods

        private void Patrolling()
        {
            _agent.speed = _speed.y;

            //if (VisionCheck())
            //{
            //    if (SquaredDistanceToTarget() < _radiusWarningSquared)
            //    {
            //        if (SquaredDistanceToTarget() < _radiusCombatSquared)
            //        {
            //            _currentState = EnemyState.Combat;
            //            return;
            //        }
            //
            //        _currentState = EnemyState.Warning;
            //        return;
            //    }
            //
            //}

            if (_agent.remainingDistance < 0.5f)
            {
                if (!_loop)
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

                }
                else
                {
                    _currentLocator = LocatorSystem.GetNextLocation(_currentRoom, _currentLocator);
                }

                _agent.SetDestination(_currentLocator.transform.position);
            }
        }

        private void Warning()
        {
            _agent.speed = _speed.x;

            //if (VisionCheck())
            //{
            //    if (SquaredDistanceToTarget() < _radiusCombatSquared)
            //    {
            //        _currentState = EnemyState.Combat;
            //        return;
            //    }
            //
            //    _currentWarning += Time.deltaTime * _warningIntensity;
            //
            //    if (_currentWarning >= 1)
            //    {
            //        _currentState = EnemyState.Combat;
            //        return;
            //    }
            //}
            //else
            //{
            //    _currentWarning -= Time.deltaTime * _warningIntensity;
            //
            //    if (_currentWarning <= 0)
            //    {
            //        _currentWarning = 0;
            //        _currentState = EnemyState.Patrol;
            //        return;
            //    }
            //}
        }

        private void Combat()
        {
            _agent.speed = _speed.z;

            _agent.SetDestination(_target.position);
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.red;

            //if (!VisionCheck())
            //{
            //    _currentLostTime += Time.deltaTime;
            //    if (_currentLostTime >= 2)
            //    {
            //        gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
            //        _currentLostTime = 0;
            //        _currentLocator = LocatorSystem.GetNearestLocation(_currentRoom, transform.position);
            //        _agent.SetDestination(_currentLocator.transform.position);
            //
            //        _currentState = EnemyState.Warning;
            //    }
            //}
            //else
            //{
            //    _currentLostTime = 0;
            //}
        }

        private bool VisionCheck()
        {
            //Debug.DrawRay(transform.position, _target.position - transform.position);
            if (Physics.Raycast(_myHead.position, _targetRaycast.position - _myHead.position, out RaycastHit hit, _radiusWarning))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Vector3 enemyToPlayer = hit.collider.transform.position - transform.position;
                    if (Vector3.Angle(transform.forward, enemyToPlayer) < 30)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion


        #region Utils

        private float SquaredDistanceToTarget()
        {
            Vector3 pos = _target.position - transform.position;
            return pos.x * pos.x + pos.y * pos.y + pos.z * pos.z;
        }

        private enum EnemyState
        {
            Patrol,
            Warning,
            Combat
        }

        #endregion


        #region Private And Protected Members

        private LocatorIdentity _currentLocator;
        bool _rePath;
        [SerializeField] private bool _loop;
        [SerializeField] private Room _currentRoom;
        [SerializeField] private EnemyState _currentState;
        [Space]
        [SerializeField] private float _radiusWarning;
        [SerializeField] private float _radiusCombat;
        private float _radiusWarningSquared;
        private float _radiusCombatSquared;

        [SerializeField] private Vector3 _speed;
        [Space]
        [SerializeField] private float _warningIntensity;
        [SerializeField] private float _currentWarning;
        [Space]
        [SerializeField] private float _currentLostTime;

        private Transform _target;
        private Transform _targetRaycast;
        [SerializeField] private Transform _myHead;
        private NavMeshAgent _agent;


        #endregion
    }
}
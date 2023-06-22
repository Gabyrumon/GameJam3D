using UnityEngine;
using UnityEngine.AI;

namespace Villager.Runtime
{
    [RequireComponent (typeof (Animator), typeof(NavMeshAgent))]
    public class DemonAI : MonoBehaviour
    {
        #region Unity API

        private void OnEnable()
        {
            SatanManager.m_instance.SpawnDemon(this);
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _agent.speed = _speed;
            HasNoTarget();
        }

        private void Update()
        {
            Combat();
            if (SatanManager.m_instance.m_villagerList.Count <= 0 && _hasTarget)
            {
                HasNoTarget();
                _anim.SetBool("Run", false);
            }
        }

        #endregion

        #region Main Methods

        private void Combat()
        {
            if (_isDead || SatanManager.m_instance.m_villagerList.Count <= 0) { _agent.isStopped = true; return; }

            if (!_hasTarget)
            {
                _target = NearestVillager();
                _agent.SetDestination(_target);
                _anim.SetBool("Run", true);
                _hasTarget = true;
            }

            if (Vector3.Distance(transform.position, _target) < 2.5f && !_attackPlayed && _hasTarget)
            {
                _anim.SetBool("Run", false);
                _anim.SetTrigger("Attack");
                _attackPlayed = true;
                _agent.isStopped = true;
            }
        }

        private Vector3 NearestVillager()
        {
            if (SatanManager.m_instance.m_villagerList.Count <= 0) return transform.position;

            nearestTarget = SatanManager.m_instance.m_villagerList[0];

            for (int i = 0; i < SatanManager.m_instance.m_villagerList.Count; i++)
            {
                VillagerAI targetToCheck = SatanManager.m_instance.m_villagerList[i];

                if (SquaredDistanceToTarget(targetToCheck.transform.position) < SquaredDistanceToTarget(nearestTarget.transform.position))
                {
                    nearestTarget = targetToCheck;
                }
            }
            return nearestTarget.transform.position;
        }

        public void KillTarget()
        {
            if (nearestTarget != null && SatanManager.m_instance.m_villagerList.Count > 0)
            {
                nearestTarget.GetComponent<VillagerAI>().ChangeState(VillagerAI.VillagerState.Dead);
            }
        }

        public void HasNoTarget()
        {
            _hasTarget = false;
            _attackPlayed = false;
            if (_agent != null)
            {
                _agent.isStopped = false;
            }
        }

        public void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _agent.isStopped = true;
            _anim.SetTrigger("Death");

            SatanManager.m_instance.DemonIsKilled(this);
        }

        #endregion


        #region Utils

        private float SquaredDistanceToTarget(Vector3 target)
        {
            Vector3 pos = target - transform.position;
            return pos.x * pos.x + pos.y * pos.y + pos.z * pos.z;
        }

        #endregion


        #region Private And Protected Members

        [SerializeField] private float _speed;

        private bool _hasTarget;
        private bool _attackPlayed;
        private bool _isDead;

        private VillagerAI nearestTarget;

        private Vector3 _target;
        private Animator _anim;
        private NavMeshAgent _agent;

        #endregion
    }
}
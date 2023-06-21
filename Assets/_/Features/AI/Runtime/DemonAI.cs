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
            HasNoTarget();
        }

        private void OnDisable()
        {
            SatanManager.m_instance.DemonIsKill(this);
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            _agent.speed = _speed;
        }

        private void Update()
        {
            Combat();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Kill Demon")) BecomeDead();
        }

        #endregion


        #region Main Methods

        private void Combat()
        {
            if (_isDead || SatanManager.m_instance.m_villagerList.Count <= 0) { _agent.isStopped = true; return; }
            //  GameOver

            if (_agent.remainingDistance < 2.5f && !_attackPlayed && _hasTarget)
            {
                _anim.SetBool("Run", false);
                _anim.SetTrigger("Attack");
                _attackPlayed = true;
                _agent.isStopped = true;
            }

            if (!_hasTarget)
            {
                _agent.SetDestination(NearestVillager());
                _anim.SetBool("Run", true);
                _hasTarget = true;
            }
        }

        private Vector3 NearestVillager()
        {
            if (SatanManager.m_instance.m_villagerList.Count <= 0) return transform.position;

            nearestTarget = SatanManager.m_instance.m_villagerList[0];

            for (int i = 0; i < SatanManager.m_instance.m_villagerList.Count; i++)
            {
                DarkSideAI targetToCheck = SatanManager.m_instance.m_villagerList[i];

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
                SatanManager.m_instance.m_villagerList.Remove(nearestTarget);
            }
        }

        public void HasNoTarget()
        {
            _hasTarget = false;
            _attackPlayed = false;
            _agent.isStopped = false;
        }

        public void BecomeDead()
        {
            _isDead = true;
            _agent.isStopped = true;
            _anim.SetTrigger("Death");
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

        private DarkSideAI nearestTarget;

        private Animator _anim;
        private NavMeshAgent _agent;

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Interaction.Runtime
{
    public class WalkToDivineIntervention : ObjectInteraction
    {
        #region Public Members

        public DivineIntervention m_divineIntervention;

        public bool m_isActive;

        #endregion

        #region Unity API

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (!m_isActive) return;
            if (_agent.remainingDistance > 0.5f) return;

            m_divineIntervention.Interact();
            m_divineIntervention = null;
            m_isActive = false;
        }

        #endregion

        #region Main Methods

        public override void PlayInteraction()
        {
            m_isActive = true;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private NavMeshAgent _agent;

        #endregion
    }
}
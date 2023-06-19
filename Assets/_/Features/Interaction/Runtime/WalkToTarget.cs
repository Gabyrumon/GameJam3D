using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Interaction.Runtime
{
    public class WalkToTarget : ObjectInteraction
    {
        #region Public Members

        public NavMeshAgent m_agent;
        public Vector3 m_target;

        #endregion

        #region Unity API

        #endregion

        #region Main Methods

        public override void PlayInteraction()
        {
            m_agent.SetDestination(m_target);
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        #endregion
    }
}
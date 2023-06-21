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

        #endregion

        #region Unity API

        private void Update()
        {
            m_divineIntervention.Interact();
            m_divineIntervention = null;
        }

        #endregion

        #region Main Methods

        public override void PlayInteraction()
        {
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        #endregion
    }
}
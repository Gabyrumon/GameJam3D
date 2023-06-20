using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction.Runtime
{
    public class DivineIntervention : MonoBehaviour
    {
        #region Public Members

        public int m_faithOrbCost;

        #endregion

        #region Unity API

        #endregion

        #region Main Methods

        public void Interact()
        {
            // Play Animation and Sound
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private AnimationClip _animationToPlay;
        private AudioClip _audioToPlay;

        #endregion
    }
}

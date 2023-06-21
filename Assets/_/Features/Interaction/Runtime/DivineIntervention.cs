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
            Debug.Log("OKKK");
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] public float _range;

        private AudioClip _audioToPlay;

        #endregion
    }
}
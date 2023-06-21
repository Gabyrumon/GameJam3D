using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction.Runtime
{
    public class InteractionManager : MonoBehaviour
    {
        #region Public Members

        public static InteractionManager m_instance;

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance != null) return;

            m_instance = this;
        }

        #endregion

        #region Main Methods

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private List<ObjectInteraction> _interactions;

        #endregion
    }
}
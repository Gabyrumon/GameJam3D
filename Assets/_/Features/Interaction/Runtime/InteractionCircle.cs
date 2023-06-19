using Inputs.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction.Runtime
{
    public class OnCircleOpenedEventArgs : EventArgs
    {
        public ObjectInteraction[] m_interactions;
    }

    public class InteractionCircle : MonoBehaviour
    {
        #region Public Members

        public EventHandler<OnCircleOpenedEventArgs> m_onCircleOpened;
        public EventHandler m_onCircleClosed;

        #endregion

        #region Unity API

        #endregion

        #region Main Methods

        public void Open()
        {
            m_onCircleOpened?.Invoke(this, new OnCircleOpenedEventArgs() { m_interactions = _interactions });
        }

        public void Close()
        {
            m_onCircleClosed?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        private ObjectInteraction[] _interactions;

        #endregion
    }
}
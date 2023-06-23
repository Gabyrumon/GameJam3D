using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sound.Runtime
{
    public class PlaySoundOnEnable : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void OnEnable()
        {
            SoundToPlay.Invoke();
        }

        #endregion

        #region Main Methods

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private UnityEvent SoundToPlay;

        #endregion
    }
}
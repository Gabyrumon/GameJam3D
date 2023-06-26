using System;
using UnityEngine;

namespace God.Runtime
{
    public class GodServiceLocator : MonoBehaviour
    {

        #region Public Members

        public static GodServiceLocator Instance { get; private set; }
        public GodInteraction GodInteraction { get; private set; }
        public GodHit GodHit { get; private set; }
        public GodJudgment GodJudgment { get; private set; }

        #endregion

        #region Unity API

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            GodInteraction = GetComponent<GodInteraction>();
            GodHit = GetComponent<GodHit>();
            GodJudgment = GetComponent<GodJudgment>();
        }

        #endregion
    }
}
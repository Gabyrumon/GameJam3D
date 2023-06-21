using System.Collections;
using UnityEngine;
using Villager.Runtime;

namespace God.Runtime
{
    public class HitSphere : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Start()
        {
            StartCoroutine(DestroyItself());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DarkSideAI _darkSide))
            {
                _darkSide.GetHit();
            }
        }

        #endregion

        #region Main Methods

        private IEnumerator DestroyItself()
        {
            yield return new WaitForSeconds(_timeToDie);
            Destroy(gameObject);
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private float _timeToDie;

        #endregion
    }
}
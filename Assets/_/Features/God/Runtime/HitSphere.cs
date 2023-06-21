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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
        }

        #endregion

        #region Main Methods

        private IEnumerator DestroyItself()
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(_timeToDie - 0.1f);
            Destroy(gameObject);
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [Range(0.1f, 1f)]
        [SerializeField] private float _timeToDie;

        #endregion
    }
}
using System.Collections;
using UnityEngine;
using Villager.Runtime;

namespace God.Runtime
{
    public class JudgmentSphere : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Start()
        {
            StartCoroutine(DisableCollider());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DarkSideAI darkSide))
            {
                if (darkSide.IsPossessed && !darkSide.GetComponentInChildren<Prayer>(true).gameObject.activeSelf)
                {
                    darkSide.GetHit();
                }
            }
            else if (other.TryGetComponent(out DemonAI demonAI))
            {
                demonAI.Die();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
        }

        #endregion

        #region Main Methods

        private IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(_cleanseDuration);
            GetComponent<Collider>().enabled = false;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [Range(0.1f, 1f)]
        [SerializeField] private float _cleanseDuration = 0.1f;

        #endregion
    }
}

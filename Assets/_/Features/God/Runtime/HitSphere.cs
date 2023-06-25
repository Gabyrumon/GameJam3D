using System.Collections;
using UnityEngine;
using Villager.Runtime;

namespace God.Runtime
{
    public class HitSphere : MonoBehaviour
    {
        #region Unity API

        private void Start()
        {
            StartCoroutine(DisableCollider());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DarkSideAI _darkSide))
            {
                if (!_darkSide.GetComponentInChildren<Prayer>(true).gameObject.activeSelf)
                {
                    _darkSide.GetHit();
                }
            }
        }

        #endregion

        #region Main Methods

        private IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(0.1f);
            GetComponent<Collider>().enabled = false;
        }

        #endregion
    }
}
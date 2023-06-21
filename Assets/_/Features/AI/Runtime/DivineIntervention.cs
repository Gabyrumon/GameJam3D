using ChurchFeature.Runtime;
using System.Collections;
using UnityEngine;

namespace Villager.Runtime
{
    public class DivineIntervention : MonoBehaviour
    {
        #region Public Members

        public int OrbCost { get => _orbCost; set => _orbCost = value; }
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }
        public int RequiredChurchLevel { get => _requiredChurchLevel; set => _requiredChurchLevel = value; }

        #endregion

        #region Unity API

        private void Start()
        {
            _church = Church.m_instance;
            _satanManager = SatanManager.m_instance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        #endregion

        #region Main Methods

        public void Interact()
        {
            foreach (var villager in _satanManager.m_villagerList)
            {
                if (Vector3.Distance(transform.position, villager.transform.position) <= _range)
                {
                    villager.GetComponent<VillagerAI>().IsConverted = true;
                    Debug.Log($"Converted {villager.gameObject.name}");
                }
            }
            StartCoroutine(StartCooldown());
        }

        private IEnumerator StartCooldown()
        {
            _isInteractable = false;
            yield return new WaitForSeconds(_cooldDown);
            _isInteractable = true;
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private int _orbCost;
        [SerializeField] private float _range;
        [SerializeField] private int _requiredChurchLevel;
        [SerializeField] private int _cooldDown;

        private Church _church;
        private SatanManager _satanManager;

        private bool _isInteractable = true;

        #endregion
    }
}
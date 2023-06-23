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
            _satanManager = SatanManager.m_instance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

        #endregion

        #region Main Methods

        public void Interact(VillagerAI source)
        {
            foreach (var villager in _satanManager.m_villagerList)
            {
                VillagerAI currentVillagerAI = villager.GetComponent<VillagerAI>();
                if (Vector3.Distance(transform.position, villager.transform.position) <= _range && currentVillagerAI != source)
                {
                    currentVillagerAI.IsConverted = true;
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

        private SatanManager _satanManager;

        private bool _isInteractable = true;

        #endregion
    }
}
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
            _filter = GetComponent<MeshFilter>();
        }

        #endregion

        #region Main Methods

        public void Interact(VillagerAI source)
        {
            foreach (var villager in _satanManager.VillagerList)
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
            _filter.mesh = _wine;
            _filter.mesh.RecalculateBounds();
            Instantiate(_VFX, Vector3.up + transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_cooldown);
            _filter.mesh = _water;
            _filter.mesh.RecalculateBounds();
            _isInteractable = true;
        }

        #endregion

        #region Private and Protected Members

        [SerializeField] private int _orbCost;
        [SerializeField] private float _range;
        [SerializeField] private int _requiredChurchLevel;
        [SerializeField] private int _cooldown;

        [Header("VFX")]
        [SerializeField] private GameObject _VFX;

        [Header("Meshes")]
        [SerializeField] private Mesh _water;

        [SerializeField] private Mesh _wine;
        private MeshFilter _filter;

        private SatanManager _satanManager;

        private bool _isInteractable = true;

        #endregion
    }
}
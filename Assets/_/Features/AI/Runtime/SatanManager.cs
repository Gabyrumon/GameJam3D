using Inputs.Runtime;
using Sound.Runtime;
using System.Collections;
using System.Collections.Generic;
using ChurchFeature.Runtime;
using UnityEngine;

namespace Villager.Runtime
{
    public class SatanManager : MonoBehaviour
    {
        #region Public Members

        public static SatanManager m_instance;

        public List<VillagerAI> m_villagerList = new List<VillagerAI>();

        public List<DarkSideAI> m_notPossessedVillagerList = new List<DarkSideAI>();

        public List<DemonAI> m_demonList = new List<DemonAI>();

        public List<VillagerAI> m_villagerHasFaithList = new List<VillagerAI>();

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetRandomTimeBeforePossession();
        }

        private void Update()
        {
            if (_timeBeforePossession > 0)
            {
                _timeBeforePossession -= Time.deltaTime;
                if (_timeBeforePossession <= 0)
                {
                    Possess();
                }
            }

            if (ChurchFeature.Runtime.Church.Instance.Level == 1 && !_satanFirstSpeachSaid)
            {
                SetRandomTimeBeforePossession();
                _firstSatanSpeach.SetActive(true);
                _satanFirstSpeachSaid = true;
            }
            if (ChurchFeature.Runtime.Church.Instance.Level == 3 && !_hasLaunchedGoWinTheGame)
            {
                GoWinTheGame();
            }

            if (m_villagerList.Count <= 0)
            {
                _loseScreen.SetActive(true);
                InputManager.m_instance.m_cantInteract = true;
                _pauseButton.SetActive(false);
            }

            if (_canVerifyIfWinTheGame  && m_demonList.Count <= 0)
            {
                _winScreen.SetActive(true);
                _winVFX.SetActive(true);
                InputManager.m_instance.m_cantInteract = true;
                _pauseButton.SetActive(false);
            }

            if (_hasLaunchedGoWinTheGame)
            {
                ChurchFeature.Runtime.Church.Instance.FaithOrbCount = 0;
            }
        }

        #endregion

        #region Main Methods

        private void SetRandomTimeBeforePossession()
        {
            _timeBeforePossession = Random.Range(_randomTimeBeforePossession[Church.Instance.Level].x, _randomTimeBeforePossession[Church.Instance.Level].y);
        }

        private void Possess()
        {
            if (m_notPossessedVillagerList.Count > 0)
            {
                int randomIndex = Random.Range(0, m_notPossessedVillagerList.Count);
                DarkSideAI current = m_notPossessedVillagerList[randomIndex];

                for (int i = 0; i < m_notPossessedVillagerList.Count; i++)
                {
                    int index = (randomIndex + i) % m_notPossessedVillagerList.Count;
                    VillagerAI currentVillager = m_notPossessedVillagerList[index].GetComponent<VillagerAI>();

                    if (currentVillager.CurrentState != VillagerAI.VillagerState.Pray)
                    {
                        current.StartPossession();
                        m_notPossessedVillagerList.RemoveAt(randomIndex);
                        break;
                    }
                }
            }

            SetRandomTimeBeforePossession();
        }

        public void SpawnDemon(DemonAI demonAI)
        {
            SoundManager.m_instance.SetPausedBattleMusic(false);
            SoundManager.m_instance.SetPausedInGameMusic(true);
            m_demonList.Add(demonAI);

            for (int i = 0; i < m_villagerList.Count; i++)
            {
                m_villagerList[i].GetComponent<VillagerAI>().ChangeState(VillagerAI.VillagerState.Afraid);
            }
        }

        public void DemonIsKilled(DemonAI demonAI)
        {
            m_demonList.Remove(demonAI);

            if (m_demonList.Count == 0)
            {
                SoundManager.m_instance.SetPausedBattleMusic(true);
                SoundManager.m_instance.SetPausedInGameMusic(false);
                for (int i = 0; i < m_villagerList.Count; i++)
                {
                    m_villagerList[i].GetComponent<VillagerAI>().ReturnToRoutine();
                }
            }
        }

        public void GoWinTheGame()
        {
            _nearToVictorySatanSpeach.SetActive(true);
            ChurchFeature.Runtime.Church.Instance.JudgmentCost = 0;

            for (int i = 0; i < m_villagerList.Count; i++)
            {
                m_villagerList[i].GetComponent<DarkSideAI>().IsPossessed = false;
                m_villagerList[i].ChangeState(VillagerAI.VillagerState.GoToChurch);
            }

            _hasLaunchedGoWinTheGame = true;
            StartCoroutine(InvokeDemons(25));
        }

        public void StartInvokingAllDemons()
        {
            _noFaithSatanSpeach.SetActive(true);
            StartCoroutine(InvokeDemons(10));
        }

        private IEnumerator InvokeDemons(int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                int index = Random.Range(0, _demonAnchors.Length);
                Instantiate(_demonPrefab, _demonAnchors[index].position, Quaternion.identity);
                yield return new WaitForSeconds(1);
            }
            if (_hasLaunchedGoWinTheGame)
            {
                _canVerifyIfWinTheGame = true;
            }
        }

        #endregion

        #region Private And Protected Members

        [Tooltip("In seconds")]
        [SerializeField] private Vector2[] _randomTimeBeforePossession;

        [Space]
        [SerializeField] private GameObject _demonPrefab;

        [SerializeField] private GameObject _firstSatanSpeach;
        [SerializeField] private GameObject _noFaithSatanSpeach;
        [SerializeField] private GameObject _nearToVictorySatanSpeach;
        [SerializeField] private Transform[] _demonAnchors;

        [Space]
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _winVFX;
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private GameObject _pauseButton;
        private float _timeBeforePossession;
        public bool _hasLaunchedGoWinTheGame;
        private bool _satanFirstSpeachSaid;
        private bool _canVerifyIfWinTheGame;

        #endregion
    }
}
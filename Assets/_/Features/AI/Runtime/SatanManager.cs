using System;
using Inputs.Runtime;
using Sound.Runtime;
using System.Collections;
using System.Collections.Generic;
using ChurchFeature.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Villager.Runtime
{
    public class SatanManager : MonoBehaviour
    {
        #region Public Members

        public static SatanManager m_instance;

        public EventHandler m_onGoWinTheGameEventHandler;

        public List<VillagerAI> VillagerList
        {
            get => _villagerList;
            set => _villagerList = value;
        }

        public List<DarkSideAI> NotPossessedVillagerList
        {
            get => _notPossessedVillagerList;
            set => _notPossessedVillagerList = value;
        }

        public List<VillagerAI> VillagerHasFaithList
        {
            get => _villagerHasFaithList;
            set => _villagerHasFaithList = value;
        }

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

            switch (ChurchManager.Instance.Level)
            {
                case 1 when !_satanFirstSpeechSaid:
                    SetRandomTimeBeforePossession();
                    _firstSatanSpeech.SetActive(true);
                    _satanFirstSpeechSaid = true;
                    break;
                case 3 when !_hasLaunchedGoWinTheGame:
                    GoWinTheGame();
                    break;
            }

            if (VillagerList.Count <= 0)
            {
                _loseScreen.SetActive(true);
                InputManager.m_instance.m_cantInteract = true;
                _pauseButton.SetActive(false);
            }

            if (_canVerifyIfWinTheGame  && _demonList.Count <= 0)
            {
                _winScreen.SetActive(true);
                _winVFX.SetActive(true);
                InputManager.m_instance.m_cantInteract = true;
                _pauseButton.SetActive(false);
            }

            if (_hasLaunchedGoWinTheGame)
            {
                ChurchManager.Instance.FaithCount = 0;
            }
        }

        #endregion

        #region Main Methods

        private void SetRandomTimeBeforePossession()
        {
            _timeBeforePossession = Random.Range(_randomTimeBeforePossession[ChurchManager.Instance.Level].x, _randomTimeBeforePossession[ChurchManager.Instance.Level].y);
        }

        private void Possess()
        {
            if (NotPossessedVillagerList.Count > 0)
            {
                int randomIndex = Random.Range(0, NotPossessedVillagerList.Count);
                DarkSideAI current = NotPossessedVillagerList[randomIndex];

                for (int i = 0; i < NotPossessedVillagerList.Count; i++)
                {
                    int index = (randomIndex + i) % NotPossessedVillagerList.Count;
                    VillagerAI currentVillager = NotPossessedVillagerList[index].GetComponent<VillagerAI>();

                    if (currentVillager.CurrentState != VillagerAI.VillagerState.Pray)
                    {
                        current.StartPossession();
                        NotPossessedVillagerList.RemoveAt(randomIndex);
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
            _demonList.Add(demonAI);

            foreach (var villager in VillagerList)
            {
                villager.GetComponent<VillagerAI>().ChangeState(VillagerAI.VillagerState.Afraid);
            }
        }

        public void DemonIsKilled(DemonAI demonAI)
        {
            _demonList.Remove(demonAI);

            if (_demonList.Count != 0) return;
            
            SoundManager.m_instance.SetPausedBattleMusic(true);
            SoundManager.m_instance.SetPausedInGameMusic(false);
            foreach (var villager in VillagerList)
            {
                villager.GetComponent<VillagerAI>().ReturnToRoutine();
            }
        }

        public void GoWinTheGame()
        {
            _nearToVictorySatanSpeech.SetActive(true);
            m_onGoWinTheGameEventHandler?.Invoke(this, EventArgs.Empty);

            foreach (var villager in VillagerList)
            {
                villager.GetComponent<DarkSideAI>().IsPossessed = false;
                villager.ChangeState(VillagerAI.VillagerState.GoToChurch);
            }

            _hasLaunchedGoWinTheGame = true;
            StartCoroutine(InvokeDemons(25));
        }

        public void StartInvokingAllDemons()
        {
            _noFaithSatanSpeech.SetActive(true);
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
        [SerializeField] private Transform[] _demonAnchors;

        [Space]
        [SerializeField] private GameObject _firstSatanSpeech;
        [SerializeField] private GameObject _noFaithSatanSpeech;
        [SerializeField] private GameObject _nearToVictorySatanSpeech;

        [Space]
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _winVFX;
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private GameObject _pauseButton;

        private List<VillagerAI> _villagerList = new();
        private List<DarkSideAI> _notPossessedVillagerList = new();
        private List<DemonAI> _demonList = new();
        private List<VillagerAI> _villagerHasFaithList = new();
        
        private float _timeBeforePossession;
        public bool _hasLaunchedGoWinTheGame;
        private bool _satanFirstSpeechSaid;
        private bool _canVerifyIfWinTheGame;

        #endregion
    }
}
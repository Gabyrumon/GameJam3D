using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Sound.Runtime
{
    public class SoundManager : MonoBehaviour
    {
        #region Public Members

        public static SoundManager m_instance;

        #endregion

        #region Unity API

        private void Awake()
        {
            if (m_instance != null) return;

            m_instance = this;

            CreateMusicInstances();
        }

        #endregion

        #region Main Methods

        #region Music

        private void CreateMusicInstances()
        {
            _mainMenuInstance = RuntimeManager.CreateInstance(_mainMenu);
            _mainMenuInstance.start();
            _mainMenuInstance.setPaused(!_playMainMenu);
            _inGameMusicInstance = RuntimeManager.CreateInstance(_inGameMusic);
            _inGameMusicInstance.start();
            _inGameMusicInstance.setPaused(_playMainMenu);
        }

        public void SetPausedMainMenu(bool isPaused)
        {
            _mainMenuInstance.setPaused(isPaused);
        }

        public void SetPausedInGameMusic(bool isPaused)
        {
            _inGameMusicInstance.setPaused(isPaused);
        }

        #endregion

        #region Click

        public void PlayClick()
        {
            RuntimeManager.PlayOneShot(_click);
        }

        #endregion

        #region UI

        public void PlayPause()
        {
            RuntimeManager.PlayOneShot(_pause);
        }

        public void PlayUnpause()
        {
            RuntimeManager.PlayOneShot(_unpause);
        }

        #endregion

        #region God

        public void PlayGodWhisper()
        {
            RuntimeManager.PlayOneShot(_godWhisper);
        }

        #endregion

        #region Church

        public void PlayChurchUpgrade()
        {
            RuntimeManager.PlayOneShot(_churchUpgrade);
        }

        #endregion

        #region Villager

        public void PlayVillagerVoiceYes(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manVoiceYes : _womanVoiceYes);
        }


        public void PlayVillagerVoiceInterrogative(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manVoiceInterrogative : _womanVoiceInterrogative);
        }

        public void PlayVillagerVoiceSurprised(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manVoiceSurprised : _womanVoiceSurprised);
        }

        public void PlayVillagerVoiceJoy(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manVoiceJoy : _womanVoiceJoy);
        }

        public void PlayVillagerVoiceSad(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manVoiceSad : _womanVoiceSad);
        }

        public void PlayVillagerVoiceHit(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manVoiceHit : _womanVoiceHit);
        }

        #endregion

        #region Possessed Villager

        public void PlaySheepDeath()
        {
            RuntimeManager.PlayOneShot(_sheepDeath);
        }

        #endregion

        #region Demon

        public void PlayDemonSpawnedSound()
        {
            RuntimeManager.PlayOneShot(_demonSpawned);
        }

        public void PlayDemonSpawnedHornSound()
        {
            RuntimeManager.PlayOneShot(_demonSpawnedHorn);
        }

        public void PlayDemonKill(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _demonKillMan : _demonKillWoman);
        }

        #endregion

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [Header("Click")]
        [SerializeField] private EventReference _click;

        [Header("Music")]
        [SerializeField] private bool _playMainMenu;
        [SerializeField] private EventReference _mainMenu;
        private EventInstance _mainMenuInstance;
        [SerializeField] private EventReference _inGameMusic;
        private EventInstance _inGameMusicInstance;

        [Header("UI")]
        [SerializeField] private EventReference _pause;
        [SerializeField] private EventReference _unpause;

        [Header("God")]
        [SerializeField] private EventReference _godWhisper;

        [Header("Church")]
        [SerializeField] private EventReference _churchUpgrade;

        [Header("Villager")]
        [SerializeField] private EventReference _manVoiceYes;
        [SerializeField] private EventReference _womanVoiceYes;
        [Space]
        [SerializeField] private EventReference _manVoiceInterrogative;
        [SerializeField] private EventReference _womanVoiceInterrogative;
        [Space]
        [SerializeField] private EventReference _manVoiceSurprised;
        [SerializeField] private EventReference _womanVoiceSurprised;
        [Space]
        [SerializeField] private EventReference _manVoiceJoy;
        [SerializeField] private EventReference _womanVoiceJoy;
        [Space]
        [SerializeField] private EventReference _manVoiceSad;
        [SerializeField] private EventReference _womanVoiceSad;
        [Space]
        [SerializeField] private EventReference _manVoiceHit;
        [SerializeField] private EventReference _womanVoiceHit;

        [Header("Possessed villager")]
        [SerializeField] private EventReference _sheepDeath;

        [Space]
        [Header("Demon")]
        [SerializeField] private EventReference _demonSpawned;
        [SerializeField] private EventReference _demonSpawnedHorn;
        [Space]
        [SerializeField] private EventReference _demonKillMan;
        [SerializeField] private EventReference _demonKillWoman;

        #endregion
    }
}

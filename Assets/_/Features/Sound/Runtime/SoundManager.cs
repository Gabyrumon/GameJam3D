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

        private void OnDestroy()
        {
            KillAllSounds();
        }

        #endregion

        #region Main Methods

        public void KillAllSounds()
        {
            _mainMenuInstance.release();
            _mainMenuInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _inGameMusicInstance.release();
            _inGameMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _battleMusicInstance.release();
            _battleMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _victoryMusicInstance.release();
            _victoryMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _defeatMusicInstance.release();
            _defeatMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        #region Music

        private void CreateMusicInstances()
        {
            _mainMenuInstance = RuntimeManager.CreateInstance(_mainMenu);
            _mainMenuInstance.start();
            _mainMenuInstance.setPaused(!_playMainMenu);

            _inGameMusicInstance = RuntimeManager.CreateInstance(_inGameMusic);
            _inGameMusicInstance.start();
            _inGameMusicInstance.setPaused(_playMainMenu);

            _battleMusicInstance = RuntimeManager.CreateInstance(_battleMusic);

            _victoryMusicInstance = RuntimeManager.CreateInstance(_victoryMusic);

            _defeatMusicInstance = RuntimeManager.CreateInstance(_defeatMusic);
        }

        public void SetPausedMainMenu(bool isPaused)
        {
            _mainMenuInstance.setPaused(isPaused);
        }

        public void SetPausedInGameMusic(bool isPaused)
        {
            _inGameMusicInstance.setPaused(isPaused);
        }

        public void SetPausedBattleMusic(bool isPaused)
        {
            _battleMusicInstance.getPlaybackState(out PLAYBACK_STATE state);
            if (isPaused && state == PLAYBACK_STATE.PLAYING)
            {
                _battleMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else if (state == PLAYBACK_STATE.STOPPED)
            {
                _battleMusicInstance.start();
            }
        }

        public void SetPausedVictoryMusic(bool isPaused)
        {
            if (isPaused)
            {
                _victoryMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else
            {
                _victoryMusicInstance.start();
            }
        }

        public void SetPausedDefeadMusic(bool isPaused)
        {
            if (isPaused)
            {
                _defeatMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else
            {
                _defeatMusicInstance.start();
            }
        }

        #endregion

        #region Buses

        public void SetMusicBusVolume(float volume)
        {
            RuntimeManager.GetBus(_musicBusPath).setVolume(volume);
        }

        public void SetSFXBusVolume(float volume)
        {
            RuntimeManager.GetBus(_sfxBusPath).setVolume(volume);
        }

        #endregion

        #region Click

        public void PlayJudgment()
        {
            RuntimeManager.PlayOneShot(_judgment);
        }

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

        public void PlayVillagerVoiceGrowl(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manGrowl : _womanGrowl);
        }

        public void PlayVillagerVoiceSlyLaugh(bool isMan)
        {
            RuntimeManager.PlayOneShot(isMan ? _manSlyLaugh : _womanSlyLaugh);
        }

        #endregion

        #region Satan

        public void PlaySatanSpeech()
        {
            RuntimeManager.PlayOneShot(_satanSpeech);
        }

        public void PlaySatanLaugh()
        {
            RuntimeManager.PlayOneShot(_satanLaugh);
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

        #region Private and Protected Members

        [Header("Buses")]
        [SerializeField] private string _musicBusPath;

        [SerializeField] private string _sfxBusPath;

        [Header("Music")]
        [SerializeField] private bool _playMainMenu;

        [SerializeField] private EventReference _mainMenu;
        private EventInstance _mainMenuInstance;
        [SerializeField] private EventReference _inGameMusic;
        private EventInstance _inGameMusicInstance;
        [SerializeField] private EventReference _battleMusic;
        private EventInstance _battleMusicInstance;
        [SerializeField] private EventReference _victoryMusic;
        private EventInstance _victoryMusicInstance;
        [SerializeField] private EventReference _defeatMusic;
        private EventInstance _defeatMusicInstance;

        [Header("God Inputs")]
        [SerializeField] private EventReference _click;

        [SerializeField] private EventReference _judgment;

        [Header("UI")]
        [SerializeField] private EventReference _pause;

        [SerializeField] private EventReference _unpause;

        [Header("God")]
        [SerializeField] private EventReference _godWhisper;

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
        [SerializeField] private EventReference _manGrowl;

        [SerializeField] private EventReference _womanGrowl;

        [Space]
        [SerializeField] private EventReference _manSlyLaugh;

        [SerializeField] private EventReference _womanSlyLaugh;

        [Header("Satan")]
        [SerializeField] private EventReference _satanSpeech;

        [SerializeField] private EventReference _satanLaugh;

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
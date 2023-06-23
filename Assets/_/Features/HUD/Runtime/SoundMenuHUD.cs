using Sound.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUD.Runtime
{
    public class SoundMenuHUD : MonoBehaviour
    {
        #region Public Members

        #endregion

        #region Unity API

        private void Awake()
        {
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChangerEventHandler);
            _sfxSlider.onValueChanged.AddListener(OnEffectsVolumeChangerEventHandler);
        }

        private void OnEnable()
        {
            SetSliderValues();
        }

        #endregion

        #region Main Methods

        private void SetSliderValues()
        {
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        private void OnMusicVolumeChangerEventHandler(float volume)
        {
            SoundManager.m_instance.SetMusicBusVolume(volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }

        private void OnEffectsVolumeChangerEventHandler(float volume)
        {
            SoundManager.m_instance.SetSFXBusVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
            PlayerPrefs.Save();
        }

        #endregion

        #region Utils

        #endregion

        #region Private and Protected Members

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        #endregion
    }
}
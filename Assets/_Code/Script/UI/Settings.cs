using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    [Header("Cache")]

    [SerializeField] private AudioMixer _am;
    private const string VOL_MUSIC = "Vol_Music";
    private const string VOL_SFX = "Vol_SFX";

    private void ChangeVolume(string group, float volume) {
        _am.SetFloat(group, MathF.Log10(volume) * 20f); // 20 + (MathF.Log10(volume) * 25f
    }

    public void ChangeMusicVolume(float volume) {
        ChangeVolume(VOL_MUSIC, volume);
    }

    public void ChangeSFXVolume(float volume) {
        ChangeVolume(VOL_SFX, volume);
    }
}

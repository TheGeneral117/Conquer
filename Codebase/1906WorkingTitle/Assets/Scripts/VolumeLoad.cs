using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeLoad : MonoBehaviour
{
    [Range(0.0001f, 1f)] private float masterVolume, musicVolume, sfxVolume;
    // Start is called before the first frame update
    void Start()
    {
        // master volume
        if(PlayerPrefs.HasKey("masterVolume"))
            masterVolume = PlayerPrefs.GetFloat("masterVolume");
        else
        {
            masterVolume = 1.0f;
            PlayerPrefs.SetFloat("masterVolume", masterVolume);
        }

        // music volume
        if (PlayerPrefs.HasKey("musicVolume"))
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        else
        {
            musicVolume = 1.0f;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
        }

        // sfx volume
        if (PlayerPrefs.HasKey("sfxVolume"))
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        else
        {
            sfxVolume = 1.0f;
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        }

        AudioListener.volume = masterVolume;

        AudioSource[] mixers = GetComponentsInParent<AudioSource>();


        if (mixers[0].outputAudioMixerGroup.audioMixer.name == "Music")
        {
            mixers[0].outputAudioMixerGroup.audioMixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
            mixers[1].outputAudioMixerGroup.audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxVolume) * 20);
        }
        else
        {
            mixers[0].outputAudioMixerGroup.audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxVolume) * 20);
            mixers[1].outputAudioMixerGroup.audioMixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
        }
    }
    
}

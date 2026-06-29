using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixers + Players")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource soundFXPlayer;

    [Header("SFX")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip victorySFX;

    public static AudioManager instance;

    // Uses singleton design pattern
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Play a given sound FX audio clip at given volume
    /// </summary>
    /// <param name="audioClip">Audio clip of SFX</param>
    /// <param name="spawnTransform">Transform of caller</param>
    /// <param name="volume">Volume to play audio clip at</param>
    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //// Spawn audio source to play sound and assign audio clip accordingly
        //AudioSource audioSource = Instantiate(soundFXPlayer, spawnTransform.position, Quaternion.identity);
        //audioSource.clip = audioClip;
        //audioSource.volume = volume;
        //audioSource.Play();

        //// Destroy clip after finished playing
        //float clipLength = audioSource.clip.length;
        //Destroy(audioSource.gameObject, clipLength);

        soundFXPlayer.PlayOneShot(audioClip);
        Debug.Log("Playing sound");
    }

    public void PlayButtonClickSound()
    {
        // Spawn game object to play sound and assign audio clip accordingly
        PlaySoundFX(buttonClickSFX, transform, 1f);
    }

    public void PlayVictorySound()
    {
        // Spawn game object to play sound and assign audio clip accordingly
        PlaySoundFX(victorySFX, transform, 0.7f);
    }
}

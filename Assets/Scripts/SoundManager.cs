using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum EffectSound { CheckNavPoint }


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioClip[] effectAudioClipArray;    

    AudioSource[] effectAudioSourceArray;
    AudioSource backgroundAudioSource;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource.rolloffMode = AudioRolloffMode.Linear;
        backgroundAudioSource.pitch = 1;
        backgroundAudioSource.minDistance = 1;
        backgroundAudioSource.maxDistance = 40;
        backgroundAudioSource.loop = true;

        effectAudioSourceArray = new AudioSource[effectAudioClipArray.Length];
        for (int i = 0; i < effectAudioClipArray.Length; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = effectAudioClipArray[i];
            audioSource.loop = false;
            audioSource.playOnAwake = false;

            effectAudioSourceArray[i] = audioSource;
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffectSound(EffectSound effectSound)
    {
        effectAudioSourceArray[(int)effectSound].Play();
    }

    public void PlayBackgroundSound(AudioClip clip)
    {
        backgroundAudioSource.clip = clip;
        backgroundAudioSource.Play();        
    }
    
    public void PlayBackgroundSound(string uri)
    {
        StartCoroutine(PlayBackgroundSoundRoutine(uri));
    }

    IEnumerator PlayBackgroundSoundRoutine(string uri)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.UNKNOWN);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }else
        {
            backgroundAudioSource.clip = DownloadHandlerAudioClip.GetContent(request);
            backgroundAudioSource.Play();
        }
            
    }

    public void StopBackgroundSound()
    {
        backgroundAudioSource.Stop();

    }
    public void SetBackgroundVolume(float volume)
    {
        backgroundAudioSource.volume = volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoObject : MonoBehaviour
{
    public string name { get; set; }
    public string description { get; set; }
    public string price { get; set; }
    public string webSiteUrl { get; set; }
    public string videoUrl { get; set; }

    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Apply settings to the video object; show the video on the material of the object.
    public void InitVideoObject(ObjectInfo _videoObjectInfo)
    {
        name = _videoObjectInfo.name;
        description = _videoObjectInfo.description;
        price = _videoObjectInfo.price;
        webSiteUrl = _videoObjectInfo.webSiteUrl;
        videoUrl = _videoObjectInfo.dataUrl;

        if (string.IsNullOrEmpty(videoUrl))
            return;

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 1.0f;
        audioSource.maxDistance = 10;
        audioSource.minDistance = 1;
        audioSource.pitch = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.isLooping = true;
        videoPlayer.url = videoUrl;
        videoPlayer.Play();
    }


}

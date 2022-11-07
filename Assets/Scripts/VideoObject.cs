using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoObject : MonoBehaviour
{
    public string name { get; set; }
    public string description { get; set; }
    public float price { get; set; }
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

    public void InitVideoObject(VideoObjectInfo _videoObjectInfo)
    {
        name = _videoObjectInfo.name;
        description = _videoObjectInfo.description;
        price = _videoObjectInfo.price;
        webSiteUrl = _videoObjectInfo.webSiteUrl;
        videoUrl = _videoObjectInfo.videoUrl;
        videoPlayer.url = videoUrl;
    }
}

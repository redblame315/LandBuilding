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
        videoPlayer.url = videoUrl;
    }
}

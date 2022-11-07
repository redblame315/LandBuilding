using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoDialog : MonoBehaviour
{
    public UIInput nameInput;
    public UIInput descriptionInput;
    public UIInput priceInput;
    public UIInput webSiteInput;
    public UIInput videoInput;

    VideoObject videoObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //init all inputs with video object info
    public void Init(VideoObject _videoObject)
    {
        videoObject = _videoObject;
        nameInput.value = _videoObject.name;
        descriptionInput.value = _videoObject.description;
        priceInput.value = _videoObject.price.ToString();
        webSiteInput.value = _videoObject.webSiteUrl;
        videoInput.value = _videoObject.videoUrl;
    }

    //appy and save settings to the object in the scene
    public void Apply()
    {
        VideoObjectInfo videoObjectInfo = new VideoObjectInfo();
        videoObjectInfo.name = nameInput.value;
        videoObjectInfo.description = descriptionInput.value;        
        videoObjectInfo.price = float.Parse(priceInput.value == "" ? "0" : priceInput.value);
        videoObjectInfo.webSiteUrl = webSiteInput.value;
        videoObjectInfo.videoUrl = videoInput.value;
        videoObject.InitVideoObject(videoObjectInfo);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VideoDialog : MonoBehaviour
{
    public InputField nameInput;
    public InputField descriptionInput;
    public InputField priceInput;
    public InputField webSiteInput;
    public InputField videoInput;
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
        nameInput.text = _videoObject.name;
        descriptionInput.text = _videoObject.description;
        priceInput.text = _videoObject.price;
        webSiteInput.text = _videoObject.webSiteUrl;
        videoInput.text = _videoObject.videoUrl;

    }

    //appy and save settings to the object in the scene
    public void Apply()
    {
        ObjectInfo videoObjectInfo = new ObjectInfo();
        videoObjectInfo.name = nameInput.text;
        videoObjectInfo.description = descriptionInput.text;        
        videoObjectInfo.price = priceInput.text;
        videoObjectInfo.webSiteUrl = webSiteInput.text;
        videoObjectInfo.dataUrl = videoInput.text;
        videoObject.InitVideoObject(videoObjectInfo);
    }
}

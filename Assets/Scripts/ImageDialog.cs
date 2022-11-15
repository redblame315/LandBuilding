using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Image Info Dialog
public class ImageDialog : MonoBehaviour
{
    public InputField nameInput;
    public InputField descriptionInput;
    public InputField priceInput;
    public InputField webSiteInput;
    public InputField imageInput;
    ImageObject imageObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Init input fields with image object info 
    public void Init(ImageObject _imageObject)
    {
        imageObject = _imageObject;
        nameInput.text = _imageObject.name;
        descriptionInput.text = _imageObject.description;
        priceInput.text = _imageObject.price;
        webSiteInput.text = _imageObject.webSiteUrl;
        imageInput.text = _imageObject.imageUrl;
    }

    //Save and apply the settings into image object
    public void Apply()
    {
        ObjectInfo imageObjectInfo = new ObjectInfo();
        imageObjectInfo.name = nameInput.text;
        imageObjectInfo.description = descriptionInput.text;
        imageObjectInfo.price = priceInput.text;
        imageObjectInfo.webSiteUrl = webSiteInput.text;
        imageObjectInfo.dataUrl = imageInput.text;
        imageObject.InitImageObject(imageObjectInfo);
    }
}

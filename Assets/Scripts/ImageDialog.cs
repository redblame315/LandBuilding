using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Image Info Dialog
public class ImageDialog : MonoBehaviour
{
    public UIInput nameInput;
    public UIInput descriptionInput;
    public UIInput priceInput;
    public UIInput webSiteInput;
    public UIInput imageInput;
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
        nameInput.value = _imageObject.name;
        descriptionInput.value = _imageObject.description;
        priceInput.value = _imageObject.price.ToString();
        webSiteInput.value = _imageObject.webSiteUrl;
        imageInput.value = _imageObject.imageUrl;
    }

    //Save and apply the settings into image object
    public void Apply()
    {
        ObjectInfo imageObjectInfo = new ObjectInfo();
        imageObjectInfo.name = nameInput.value;
        imageObjectInfo.description = descriptionInput.value;
        imageObjectInfo.price = float.Parse(priceInput.value);
        imageObjectInfo.webSiteUrl = webSiteInput.value;
        imageObjectInfo.dataUrl = imageInput.value;
        imageObject.InitImageObject(imageObjectInfo);
    }
}

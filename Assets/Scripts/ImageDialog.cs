using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Init(ImageObject _imageObject)
    {
        imageObject = _imageObject;
        nameInput.value = _imageObject.name;
        descriptionInput.value = _imageObject.description;
        priceInput.value = _imageObject.price.ToString();
        webSiteInput.value = _imageObject.webSiteUrl;
        imageInput.value = _imageObject.imageUrl;
    }

    public void Apply()
    {
        ImageObjectInfo imageObjectInfo = new ImageObjectInfo();
        imageObjectInfo.name = nameInput.value;
        imageObjectInfo.description = descriptionInput.value;
        imageObjectInfo.price = float.Parse(priceInput.value);
        imageObjectInfo.webSiteUrl = webSiteInput.value;
        imageObjectInfo.imageUrl = imageInput.value;
        imageObject.InitImageObject(imageObjectInfo);
    }
}

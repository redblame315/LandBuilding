using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestImageDIalog : Dialog
{
    public static GuestImageDIalog instance = null;
    public UILabel nameLabel;
    public UILabel descriptionLabel;
    public UILabel priceLabel;
    public UILabel webSiteUrlLabel;
    public UILabel imageUrlLabel;
    public void Awake()
    {
        instance = this;
    }
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
        nameLabel.text = _imageObject.name;
        descriptionLabel.text = _imageObject.description;
        priceLabel.text = _imageObject.price.ToString();
        webSiteUrlLabel.text = _imageObject.webSiteUrl;
        imageUrlLabel.text = _imageObject.imageUrl;
        SetVisible(true);
    }
}

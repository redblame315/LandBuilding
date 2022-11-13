using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestVideoDIalog : Dialog
{
    public static GuestVideoDIalog instance = null;
    public UILabel nameLabel;
    public UILabel descriptionLabel;
    public UILabel priceLabel;
    public UILabel webSiteUrlLabel;
    public UILabel videoUrlLabel;

    private void Awake()
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

    public void Init(VideoObject _VideoObject)
    {
        nameLabel.text = _VideoObject.name;
        descriptionLabel.text = _VideoObject.description;
        priceLabel.text = _VideoObject.price.ToString();
        webSiteUrlLabel.text = _VideoObject.webSiteUrl;
        videoUrlLabel.text = _VideoObject.videoUrl;
        SetVisible(true);
    }
}

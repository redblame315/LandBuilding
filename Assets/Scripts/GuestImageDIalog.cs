using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuestImageDIalog : Dialog
{
    public UILabel nameLabel;
    public UILabel descriptionLabel;
    public UILabel priceLabel;
    public UILabel imageUrlLabel;
    public GameObject webSiteIconObj;

    string webSiteUrl;

    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);
    public void Awake()
    {
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
        if (MainScreen.instance.curGuestTransformDialog != null)
            MainScreen.instance.curGuestTransformDialog.SetVisible(false);

        MainScreen.instance.curGuestTransformDialog = this;
        nameLabel.text = _imageObject.name;
        descriptionLabel.text = _imageObject.description;
        priceLabel.text = _imageObject.price;
        webSiteUrl = _imageObject.webSiteUrl;
        webSiteIconObj.SetActive(!string.IsNullOrEmpty(webSiteUrl));
        imageUrlLabel.text = _imageObject.imageUrl;
        SetVisible(true);
    }

    public void WebSiteLinkClicked()
    {

#if !UNITY_EDITOR && UNITY_WEBGL
        if(!string.IsNullOrEmpty(webSiteUrl))
            OpenNewTab(webSiteUrl);        
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuestVideoDialog : Dialog
{
    public UILabel nameLabel;
    public UILabel descriptionLabel;
    public UILabel priceLabel;
    public UILabel webSiteUrlLabel;
    public UILabel videoUrlLabel;

    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);

    private void Awake()
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

    public void Init(VideoObject _VideoObject)
    {
        nameLabel.text = _VideoObject.name;
        descriptionLabel.text = _VideoObject.description;
        priceLabel.text = _VideoObject.price;
        webSiteUrlLabel.text = _VideoObject.webSiteUrl;
        videoUrlLabel.text = _VideoObject.videoUrl;
        SetVisible(true);
    }

    public void WebSiteLinkClicked()
    {

#if !UNITY_EDITOR && UNITY_WEBGL
        if(!string.IsNullOrEmpty(webSiteUrlLabel.text))
            OpenNewTab(webSiteUrlLabel.text);        
#endif
    }
}

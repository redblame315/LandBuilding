using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuestVideoDIalog : Dialog
{
    public UILabel nameLabel;
    public UILabel descriptionLabel;
    public UILabel priceLabel;
    public UILabel videoUrlLabel;
    public GameObject webSiteIconObj;

    string webSiteUrl;

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
        if (MainScreen.instance.curGuestTransformDialog != null)
            MainScreen.instance.curGuestTransformDialog.SetVisible(false);

        MainScreen.instance.curGuestTransformDialog = this;

        nameLabel.text = _VideoObject.name;
        descriptionLabel.text = _VideoObject.description;
        priceLabel.text = _VideoObject.price;
        webSiteUrl = _VideoObject.webSiteUrl;
        webSiteIconObj.SetActive(!string.IsNullOrEmpty(webSiteUrl));
        videoUrlLabel.text = _VideoObject.videoUrl;
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

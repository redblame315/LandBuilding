using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Image Object
public class ImageObject : MonoBehaviour
{
    public MeshRenderer imageRenderer;
    public string name { get; set; }
    public string description { get; set; }
    public string price { get; set; }
    public string webSiteUrl { get; set; }
    public string imageUrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Apply image object settings to the object.
    public void InitImageObject(ObjectInfo _imageObjectInfo)
    {
        name = _imageObjectInfo.name;
        description = _imageObjectInfo.description;
        price = _imageObjectInfo.price;
        webSiteUrl = _imageObjectInfo.webSiteUrl;
        imageUrl = _imageObjectInfo.dataUrl;

        if (string.IsNullOrEmpty(imageUrl))
            return;

        if (!GameManager.instance.forAdmin)
            imageUrl = imageUrl.Replace("admin", "guest");

        StartCoroutine(DownloadImage(imageUrl));
    }

    //Download and show image from url
    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Material standardShaderMaterial = new Material(Shader.Find("Standard"));
            standardShaderMaterial.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            //Set Rendering Mode to Cutout
            standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            standardShaderMaterial.SetInt("_ZWrite", 1);
            standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
            standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
            standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            standardShaderMaterial.renderQueue = 2450;

            imageRenderer.sharedMaterial = standardShaderMaterial;
        }
    }

    private void OnMouseEnter()
    {
        MainScreen.instance.descriptionDialog.Init(name, description);
    }

    private void OnMouseExit()
    {
        MainScreen.instance.descriptionDialog.SetVisible(false);
    }
}

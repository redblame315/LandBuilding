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
    public float price { get; set; }
    public string webSiteUrl { get; set; }
    public string imageUrl { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Apply image object settings to the object.
    public void InitImageObject(ImageObjectInfo _imageObjectInfo)
    {
        name = _imageObjectInfo.name;
        description = _imageObjectInfo.description;
        price = _imageObjectInfo.price;
        webSiteUrl = _imageObjectInfo.webSiteUrl;
        imageUrl = _imageObjectInfo.imageUrl;
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
            Material material = new Material(Shader.Find("Standard"));
            material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            imageRenderer.sharedMaterial = material;
        }
    }
}

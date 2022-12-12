using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Transfrom Dialog related to the object transform
public class TransformDialog : Dialog
{
    public InputField positionXInput;
    public InputField positionYInput;
    public InputField positionZInput;

    public InputField rotationXInput;
    public InputField rotationYInput;
    public InputField rotationZInput;

    public InputField scaleXInput;
    public InputField scaleYInput;
    public InputField scaleZInput;

    public bool isBusy = false;

    Transform targetTransform;
    Vector3 initPos = Vector3.zero;
    Vector3 initScale = Vector3.one;
    Vector3 initRotation = Vector3.zero;

    string prefabName;

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

    //Init all input with the target object info
    public void SetTarget(Transform _target, string _prefabName)
    {
        isBusy = true;

        if (MainScreen.instance.curTransformDialog != null)
            MainScreen.instance.curTransformDialog.SetVisible(false);

        MainScreen.instance.curTransformDialog = this;
        targetTransform = _target;
        initPos = targetTransform.localPosition;
        initRotation = targetTransform.localEulerAngles;
        initScale = targetTransform.localScale;

        prefabName = _prefabName;

        positionXInput.text = targetTransform.localPosition.x.ToString();
        positionYInput.text = targetTransform.localPosition.y.ToString();
        positionZInput.text = targetTransform.localPosition.z.ToString();

        rotationXInput.text = targetTransform.localEulerAngles.x.ToString();
        rotationYInput.text = targetTransform.localEulerAngles.y.ToString();
        rotationZInput.text = targetTransform.localEulerAngles.z.ToString();

        scaleXInput.text = targetTransform.localScale.x.ToString();
        scaleYInput.text = targetTransform.localScale.y.ToString();
        scaleZInput.text = targetTransform.localScale.z.ToString();

        ImageObject imageObject = targetTransform.GetComponent<ImageObject>();        
        if (imageObject)
        {
            ImageDialog imageDialog = gameObject.GetComponent<ImageDialog>();
            imageDialog.Init(imageObject);
        }

        VideoObject videoObject = targetTransform.GetComponent<VideoObject>();        
        if (videoObject)
        {
            VideoDialog videoDialog = gameObject.GetComponent<VideoDialog>();
            videoDialog.Init(videoObject);
        }

        SetVisible(true);
        isBusy = false;
    }

    //save button action; apply settings to the object in the scene and save info into firestore
    public void SaveButtonClicked()
    {
        string tag = targetTransform.tag;
        ObjectInfo objectInfo = new ObjectInfo();
        if (tag == "NormalObject")
        {
            objectInfo.objectType = ObjectType.Normal;
        }
        else if (tag == "ImageObject")
        {
            ImageDialog imageDialog = gameObject.GetComponent<ImageDialog>();
            //apply settings to the object in the scene
            imageDialog.Apply();

            ImageObject imageObject = targetTransform.GetComponent<ImageObject>();
            objectInfo.name = imageObject.name;
            objectInfo.description = imageObject.description;
            objectInfo.price = imageObject.price;
            objectInfo.webSiteUrl = imageObject.webSiteUrl;
            objectInfo.dataUrl = imageObject.imageUrl;

            objectInfo.objectType = ObjectType.Image;
        }
        else 
        {
            VideoDialog videoDialog = gameObject.GetComponent<VideoDialog>();
            //apply settings to the object in the scene
            videoDialog.Apply();

            VideoObject videoObject = targetTransform.GetComponent<VideoObject>();
            objectInfo.name = videoObject.name;
            objectInfo.description = videoObject.description;
            objectInfo.price = videoObject.price;
            objectInfo.webSiteUrl = videoObject.webSiteUrl;
            objectInfo.dataUrl = videoObject.videoUrl;

            objectInfo.objectType = ObjectType.Video;
        }

        objectInfo.objectId = targetTransform.name;
        objectInfo.prefabName = prefabName;        
        objectInfo.position = JsonUtility.ToJson(targetTransform.localPosition);
        objectInfo.rotation = JsonUtility.ToJson(targetTransform.localEulerAngles);
        objectInfo.scale = JsonUtility.ToJson(targetTransform.localScale);
        objectInfo.posState = (int)HeroCtrl.instance.heroPosState;
        //save setting info into firestore
        DBManager.Instance().SaveObjectByFireStore(objectInfo);
        CloseButtonClicked();
    }

    public void PositionValueChanged()
    {
        if (isBusy)
            return;

        float x = float.Parse(String.IsNullOrEmpty(positionXInput.text) ? "0" : positionXInput.text);
        float y = float.Parse(String.IsNullOrEmpty(positionYInput.text) ? "0" : positionYInput.text);
        float z = float.Parse(String.IsNullOrEmpty(positionZInput.text) ? "0" : positionZInput.text);
        targetTransform.localPosition = new Vector3(x, y, z);
    }

    public void RotationValueChanged()
    {
        if (isBusy)
            return;

        try
        {            
            float x = float.Parse(String.IsNullOrEmpty(rotationXInput.text) ? "0" : rotationXInput.text);
            float y = float.Parse(String.IsNullOrEmpty(rotationYInput.text) ? "0" : rotationYInput.text);
            float z = float.Parse(String.IsNullOrEmpty(rotationZInput.text) ? "0" : rotationZInput.text);
            targetTransform.localEulerAngles = new Vector3(x, y, z);
        }
        catch(Exception e)
        {

        }
        
    }

    public void ScaleValueChanged()
    {
        if (isBusy)
            return;
                
        float x = float.Parse(String.IsNullOrEmpty(scaleXInput.text) ? "0" : scaleXInput.text);
        float y = float.Parse(String.IsNullOrEmpty(scaleYInput.text) ? "0" : scaleYInput.text);
        float z = float.Parse(String.IsNullOrEmpty(scaleZInput.text) ? "0" : scaleZInput.text);
        targetTransform.localScale = new Vector3(x, y, z);
    }

    public void CancelButtonClicked()
    {
        targetTransform.localPosition = initPos;
        targetTransform.localEulerAngles = initRotation;
        targetTransform.localScale = initScale;
        CloseButtonClicked();
    }

    public void DeleteButtonClicked()
    {
        ObjectInfo objectInfo = new ObjectInfo();
        objectInfo.objectId = targetTransform.name;
        objectInfo.prefabName = "";

        DBManager.Instance().SaveObjectByFireStore(objectInfo);
        Destroy(targetTransform.gameObject);
        CloseButtonClicked();
    }
}

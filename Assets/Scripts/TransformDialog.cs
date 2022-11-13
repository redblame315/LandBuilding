using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Transfrom Dialog related to the object transform
public class TransformDialog : Dialog
{
    public static TransformDialog instance = null;
    public UIInput positionXInput;
    public UIInput positionYInput;
    public UIInput positionZInput;

    public UIInput rotationXInput;
    public UIInput rotationYInput;
    public UIInput rotationZInput;

    public UIInput scaleXInput;
    public UIInput scaleYInput;
    public UIInput scaleZInput;

    public bool isBusy = false;
    public GameObject applyButton;

    Transform targetTransform;
    string prefabName;

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

    //Init all input with the target object info
    public void SetTarget(Transform _target, string _prefabName)
    {
        isBusy = true;
        targetTransform = _target;
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

        applyButton.SetActive(GameManager.instance.forAdmin);
        SetVisible(true);

        isBusy = false;
    }

    public void SetVisible(bool visible)
    {
        instance = this;
        transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    public bool GetVisible()
    {
        return transform.localScale.x == 1;
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
        SetVisible(false);
    }

    public void PositionValueChanged()
    {
        if (isBusy)
            return;

        float x = float.Parse(positionXInput.value);
        float y = float.Parse(positionYInput.value);
        float z = float.Parse(positionZInput.value);
        targetTransform.localPosition = new Vector3(x, y, z);
    }

    public void RotationValueChanged()
    {
        if (isBusy)
            return;

        try
        {
            float x = float.Parse(rotationXInput.value);
            float y = float.Parse(rotationYInput.value);
            float z = float.Parse(rotationZInput.value);
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

        float x = float.Parse(scaleXInput.value);
        float y = float.Parse(scaleYInput.value);
        float z = float.Parse(scaleZInput.value);
        targetTransform.localScale = new Vector3(x, y, z);
    }
}

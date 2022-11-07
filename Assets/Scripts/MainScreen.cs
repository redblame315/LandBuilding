using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : UIScreen
{
    public static MainScreen instance = null;
    public GameObject landObj;
    public UILabel landTitleLabel;
    public TransformDialog normalObjectInfoDialog;
    public TransformDialog imageObjectInfoDialog;
    public TransformDialog videoObjectInfoDialog;

    private void Awake()
    {
        instance = this;
    }

    //Init actions when the mainscreen is loaded
    public override void Init()
    {
        //Show the ground and Activate fps
        landObj.SetActive(true);
        //Show the user's info
        landTitleLabel.text = DBManager.Instance().userInfo.username + "'s Land";
        //Load objects from firestore databse
        DBManager.Instance().LoadObjectsByFirestore();
        GameManager.instance.bStart = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Load all objects with objectlist info from firestore.
    public void InitObjects(List<ObjectInfo> objectInfoList)
    {
        for(int i = 0; i < objectInfoList.Count; i++)
        {
            ObjectInfo objectInfo = objectInfoList[i];
            GameObject prefab = Resources.Load("Prefabs/" + objectInfo.prefabName) as GameObject;
            GameObject obj = Instantiate(prefab) as GameObject;
            obj.layer = LayerMask.NameToLayer("Object");
            obj.name = objectInfo.objectId;            
            obj.transform.parent = landObj.transform;
            obj.transform.localPosition = JsonUtility.FromJson<Vector3>(objectInfo.position);
            obj.transform.localEulerAngles = JsonUtility.FromJson<Vector3>(objectInfo.rotation);
            obj.transform.localScale = JsonUtility.FromJson<Vector3>(objectInfo.scale);
            
            if(obj.tag == "ImageObject")
            {
                ImageObject imageObject = obj.GetComponent<ImageObject>();
                imageObject.InitImageObject((ImageObjectInfo)objectInfo);
            }else if(obj.tag == "VideoObject")
            {
                VideoObject videoObject = obj.GetComponent<VideoObject>();
                videoObject.InitVideoObject((VideoObjectInfo)objectInfo);
            }
        }
    }

    public void LogOutButtonClicked()
    {
        SceneManager.LoadSceneAsync(0);        
    }
}

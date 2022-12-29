using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStartState { None, Logout, EnterInterior, ExitInterior}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    DBManager dbManager = null;
    public bool bStart = false;
    [HideInInspector]
    public GameObject curPrefabObject = null;

    public GameObject[] dontDestoryOnLoadObjArray;
    public static GameStartState gameStartState = GameStartState.None;
    public bool forAdmin = true;
    public bool forAskAccountName = true;
    public bool ecom = true;
    public bool ecomClick = true;
    public bool ecomCameraMove = true;
    public string adminUserId = "aman";
    public string adminUserName = "aman";
    

    private void Awake()
    {
        instance = this;
        //ClearDontDestroyOnLoad();
    }
    // Start is called before the first frame update
    void Start()
    {        
        dbManager = DBManager.Instance();        
    }

    public void ClearDontDestroyOnLoad()
    {
        Debug.LogError("ClearDontDestroyOnLoad : Begin");
        var go = new GameObject("go");
        DontDestroyOnLoad(go);

        foreach (var root in go.scene.GetRootGameObjects())
        {
            Debug.LogError("DonDestroyObjects Name : " + root.name);
#if !UNITY_WEBGL || UNITY_EDITOR
            if (!root.name.Contains("DB") && !root.name.Contains("Fire") && !root.name.Contains("RealTime"))
                Destroy(root);
#else
            Destroy(root);
#endif
        }

        Debug.LogError("ClearDontDestroyOnLoad : End");
    }

    public void InitDontDestroyOnLoad()
    {        
        bool destroy = instance != null;
        for (int i = 0; i < dontDestoryOnLoadObjArray.Length; i++)
            DontDestroyOnLoad(dontDestoryOnLoadObjArray[i]);

        /*if (GameManager.instance)
        {
            switch (GameManager.instance.gameStartState)
            {
                case GameStartState.Logout:
                    UIManager.instance.Init();
                    MainScreen.instance.InitRoad();
                    break;

                case GameStartState.ExitInterior:
                    MainScreen.instance.ResetFrontSettings();
                    break;
            }            
        }*/
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0) && !NGUIMousePoint.bGrabMouse)
        {
            if (MainScreen.instance.curTransformDialog != null && MainScreen.instance.curTransformDialog.GetVisible())
                return;

            if (HeroCtrl.isMovingTarget)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (!hit.collider.tag.Contains("Object"))
                    return;

                curPrefabObject = hit.collider.gameObject;
            }
        }
            //Check the mouse click on object on the scene and open info dialog
        if (Input.GetMouseButtonUp(0))
        {
            NGUIMousePoint.bGrabMouse = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (!hit.collider.tag.Contains("Object") || hit.collider.gameObject != curPrefabObject)
                {
                    curPrefabObject = null;
                    return;
                }

                if (ecomCameraMove)
                    HeroCtrl.instance.MoveToTarget(hit.collider.transform);
                else
                    ShowTransformDialog(hit.collider.gameObject);
            }
            curPrefabObject = null;
        }

        if(ecom && !MainScreen.instance.guestImageDialog.GetVisible() && !MainScreen.instance.guestVideoDialog.GetVisible())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                string tag = hit.collider.tag;
                string name = "";
                string description = "";
                if (tag == "ImageObject")
                {
                    ImageObject imageObject = hit.collider.GetComponent<ImageObject>();
                    name = imageObject.name;
                    description = imageObject.description;
                }else if(tag == "VideoObject")
                {
                    VideoObject imageObject = hit.collider.GetComponent<VideoObject>();
                    name = imageObject.name;
                    description = imageObject.description;
                }

                if(!string.IsNullOrEmpty(name))
                    MainScreen.instance.descriptionDialog.Init(name,description);
                else
                    MainScreen.instance.descriptionDialog.SetVisible(false);
            }
            else
                MainScreen.instance.descriptionDialog.SetVisible(false);
        }else
            MainScreen.instance.descriptionDialog.SetVisible(false);

    }

    public void ShowTransformDialog(GameObject hitObj)
    {
        string objName = hitObj.name;
        string[] subNameList = objName.Split("_");
        string prefabName = "";
        if (subNameList.Length > 0)
            prefabName = subNameList[0];
        if (forAdmin)
        {
            if (hitObj.tag == "NormalObject")
            {
                MainScreen.instance.normalObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
            }
            else if (hitObj.tag == "ImageObject")
                MainScreen.instance.imageObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
            else if (hitObj.tag == "VideoObject")
                MainScreen.instance.videoObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
        }
        else if (ecomClick)
        {
            if (hitObj.tag == "ImageObject")
            {
                ImageObject imageObject = hitObj.GetComponent<ImageObject>();
                MainScreen.instance.guestImageDialog.Init(imageObject);
            }
            else if (hitObj.tag == "VideoObject")
            {
                VideoObject videoObject = hitObj.GetComponent<VideoObject>();
                MainScreen.instance.guestVideoDialog.Init(videoObject);
            }
        }

    }
}

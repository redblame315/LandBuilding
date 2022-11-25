using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    DBManager dbManager = null;
    public bool bStart = false;
    [HideInInspector]
    public GameObject curPrefabObject = null;
      
    public bool forAdmin = true;
    public bool forAskAccountName = true;
    public string adminUserId = "aman";
    public string adminUserName = "aman";

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        dbManager = DBManager.Instance();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0) && !NGUIMousePoint.bGrabMouse)
        {
            if (MainScreen.instance.curTransformDialog != null && MainScreen.instance.curTransformDialog.GetVisible())
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
                    

                GameObject hitObj = hit.collider.gameObject;
                string objName = hitObj.name;
                string[] subNameList = objName.Split("_");
                string prefabName = "";
                if (subNameList.Length > 0)
                    prefabName = subNameList[0];
                if(forAdmin)
                {
                    if (hitObj.tag == "NormalObject")
                    {
                        MainScreen.instance.normalObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
                    }
                    else if (hitObj.tag == "ImageObject")
                        MainScreen.instance.imageObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
                    else if (hitObj.tag == "VideoObject")
                        MainScreen.instance.videoObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
                }else
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
            curPrefabObject = null;
        }
    }
}

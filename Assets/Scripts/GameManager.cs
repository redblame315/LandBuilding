using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    DBManager dbManager = null;
    public bool bStart = false;
    private void Awake()
    {
        dbManager = new DBManager();
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0) && !NGUIMousePoint.bGrabMouse)
        {
            if (TransformDialog.instance == null || TransformDialog.instance.GetVisible())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if(Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Object")))
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (!hit.collider.tag.Contains("Object"))
                    return;

                GameObject hitObj = hit.collider.gameObject;
                string objName = hitObj.name;
                string[] subNameList = objName.Split("_");
                string prefabName = "";
                if (subNameList.Length > 0)
                    prefabName = subNameList[0];
                if(hitObj.tag == "NormalObject")
                    MainScreen.instance.normalObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
                else if(hitObj.tag == "ImageObject")
                    MainScreen.instance.imageObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
                else if(hitObj.tag == "VideoObject")
                    MainScreen.instance.videoObjectInfoDialog.SetTarget(hitObj.transform, prefabName);
            }
        }

        if (Input.GetMouseButtonUp(0))
            NGUIMousePoint.bGrabMouse = false;
    }
}

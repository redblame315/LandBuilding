using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    DBManager dbManager = null;

    private void Awake()
    {
        dbManager = new DBManager();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !TransformDialog.instance.GetVisible())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if(Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Object")))
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Object"))
                    return;

                GameObject hitObj = hit.collider.gameObject;
                string objName = hitObj.name;
                string[] subNameList = objName.Split("_");
                string prefabName = "";
                if (subNameList.Length > 0)
                    prefabName = subNameList[0];
                TransformDialog.instance.SetTarget(hitObj.transform, prefabName);
            }
        }
        
    }
}

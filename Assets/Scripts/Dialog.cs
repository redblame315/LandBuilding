using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetVisible(bool visible)
    {
        transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    public bool GetVisible()
    {
        return transform.localScale.x == 1;
    }

    public void CloseButtonClicked()
    {
        SetVisible(false);
    }
}

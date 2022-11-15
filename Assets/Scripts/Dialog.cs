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
        gameObject.SetActive(visible);
    }

    public bool GetVisible()
    {
        return gameObject.activeSelf;
    }

    public void CloseButtonClicked()
    {
        SetVisible(false);
    }
}

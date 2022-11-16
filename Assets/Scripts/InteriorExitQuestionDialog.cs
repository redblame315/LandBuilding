using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorExitQuestionDialog : Dialog
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OkButtonClicked()
    {
        MainScreen.instance.ExitInterior();
        gameObject.SetActive(false);
    }
}

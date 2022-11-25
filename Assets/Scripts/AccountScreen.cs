using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountScreen : UIScreen
{
    public UIInput accountInput;

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

    public void OkButtonClicked()
    {
        if (string.IsNullOrEmpty(accountInput.value))
            return;

        DBManager.Instance().LoginUserByFireStore(accountInput.value, "");
    }

    public override void Init()
    {
    }
}

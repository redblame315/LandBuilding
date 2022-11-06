using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignupScreen : UIScreen
{
    public UIInput userIdInput;
    public UIInput userNameInput;
    public UIInput passwordInput;

    public override void Init()
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

    public void CloseButtonClicked()
    {
        UIManager.instance.loginScreen.Focus();
    }

    public void RegisterButtonClicked()
    {
        DBManager.Instance().SinupUserByFireStore(userIdInput.value, userNameInput.value, passwordInput.value);
    }
}

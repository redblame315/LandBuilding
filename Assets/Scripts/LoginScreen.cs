using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreen : UIScreen
{
    public UIInput userIdInput;
    public UIInput passwordInput;

    private DBManager dbManger = null;

    private void Awake()
    {
        dbManger = DBManager.Instance();
    }
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

    public void LoginButtonClicked()
    {
        string userId = userIdInput.value;
        string password = passwordInput.value;
        //dbManger.LoginUser(userId, password);
        dbManger.LoginUserByFireStore(userId, password);
    }

    public void SignUpButtonClicked()
    {
        UIManager.instance.signupScreen.Focus();
    }

    
}

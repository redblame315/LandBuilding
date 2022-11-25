using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public UIScreen signupScreen;
    public UIScreen loginScreen;
    public UIScreen accountScreen;
    public UIScreen mainUIScreen;
    public bool bBusy = false;
    private UserInfo userInfo;
    private GameManager gameManager;
    private Queue<Action> _actionQueue = new Queue<Action>();

    private void Awake()
    {
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.forAdmin)
            loginScreen.Focus();
        else
        {
            if (GameManager.instance.forAskAccountName)
                accountScreen.Focus();
            else
                mainUIScreen.Focus();
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        while(_actionQueue.Count > 0)
        {
            Action action;
            lock(_actionQueue)
            {
                action = _actionQueue.Dequeue();
            }

            action();
        }
    }    

    public void EnqueueAction(Action action)
    {
        lock(_actionQueue)
        {
            _actionQueue.Enqueue(action);
        }
    }

    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public UIScreen signupScreen;
    public UIScreen loginScreen;
    public UIScreen mainUIScreen;
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
        loginScreen.Focus();
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

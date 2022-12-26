using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public UIScreen signupScreen;
    public UIScreen loginScreen;
    public UIScreen accountScreen;
    public UIScreen accountWebScreen;
    public UIScreen mainUIScreen;
    public bool bBusy = false;
    private UserInfo userInfo;
    private GameManager gameManager;
    private Queue<Action> _actionQueue = new Queue<Action>();

    [DllImport("__Internal")]
    private static extern string GetURLFromPage();
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (GameManager.instance.forAdmin)
        {
            if (string.IsNullOrEmpty(DBManager.userInfo.userId))
            {
                Debug.LogError("UIManager Init : Begine");
                loginScreen.Focus();
                Debug.LogError("UIManager Init : End");
            }
            else
                DBManager.Instance().LoginUserByFireStore(DBManager.userInfo.userId, "");
        }
        else
        {
            if (GameManager.instance.forAskAccountName)
            {

#if !UNITY_WEBGL || UNITY_EDITOR
                accountScreen.Focus();
                //accountWebScreen.Focus();
#else
                //accountWebScreen.Focus();
                string url = GetURLFromPage();
                int startIndex = url.LastIndexOf("/") + 2;
                if(startIndex < url.Length)
                {
                    string userId = url.Substring(startIndex);
                    DBManager.Instance().LoginUserByFireStore(userId, "");
                }   
#endif
            }
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

    public static Vector3 convertMousePos2NGUIPos(Vector3 mousePos)
    {
        float nguiWidth = 1080f * Screen.width / Screen.height;
        float nguiHeight = 1080f;

        float wRate = nguiWidth / Screen.width;
        float hRate = nguiHeight / Screen.height;

        float nguiY = (mousePos.y - Screen.height / 2) * hRate;
        float nguiX = (mousePos.x - Screen.width / 2) * wRate;

        Vector3 nguiPos = new Vector3(nguiX, nguiY, 0);
        return nguiPos;
    }
}

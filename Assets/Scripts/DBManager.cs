using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;
#else
using FirebaseWebGL.Scripts;
#endif

public enum ObjectType { Normal, Image, Video }
public class UserInfo {
    public string userId;
    public string username;
    public string password;

    public UserInfo()
    {

    }

    public UserInfo(string userId, string username, string password)
    {
        this.userId = userId;
        this.username = username;
        this.password = password;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["username"] = username;
        result["password"] = password;
        return result;
    }
}

public class ObjectInfo
{
    public string objectId;
    public ObjectType objectType = ObjectType.Normal;
    public string prefabName;
    public string position;
    public string rotation;
    public string scale;
    public int posState;
    public string name = "";
    public string description = "";
    public string price = "";
    public string webSiteUrl = "";
    public string dataUrl = "";

    public ObjectInfo()
    { 
    }
    
}

public class CSettingInfo
{
    public float bgvolume = 1f;
    public string bgsong = "";
    public string sfront = "";
    public string sinterior = "";

    public CSettingInfo()
    {

    }

    public CSettingInfo(string _bgsong)
    {
        bgsong = _bgsong;
    }
}

public class DBManager : MonoBehaviour
{
    static DBManager dbManager = null;
#if !UNITY_WEBGL || UNITY_EDITOR
    DatabaseReference mDatabase;
    FirebaseDatabase mFireBaseDatabase;
    FirebaseFirestore mFirebaseFireStore;
#endif
    public static UserInfo userInfo = new UserInfo();
    public static CSettingInfo cSettingInfo = new CSettingInfo();

    string curUserId;
    private void Awake()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (dbManager == null)
        {
            dbManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
#else
        dbManager = this;
        DontDestroyOnLoad(gameObject);
#endif
#if !UNITY_WEBGL || UNITY_EDITOR
        //mFireBaseDatabase = FirebaseDatabase.GetInstance("https://landbuilding-5644c-default-rtdb.firebaseio.com");
        try
        {
            mFirebaseFireStore = FirebaseFirestore.DefaultInstance;
        }
        catch(Exception exception)
        {
            Debug.LogError("Firestore Init Error" + exception.Message);
            mFirebaseFireStore.TerminateAsync();
        }


        //mFirebaseFireStore.DisableNetworkAsync();
        //mFirebaseFireStore.EnableNetworkAsync();
        //mDatabase = mFireBaseDatabase.RootReference;
#endif
    }

    public static DBManager Instance()
    {
        //if (dbManager == null)
        //    dbManager = new DBManager();

        return dbManager;
    }

    //Check user login with usre id and password in the firestore database
    public void LoginUserByFireStore(string userId, string password)
    {
        userInfo.userId = userId;
        userInfo.password = password;
#if !UNITY_WEBGL || UNITY_EDITOR
        mFirebaseFireStore.Collection("Users")
            .Document(userId)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task => {
                DocumentSnapshot snapshot = task.Result;
                Dictionary<string, object> userData = snapshot.ToDictionary();
                if (userData == null)
                    return;

                if(string.IsNullOrEmpty(password))
                {
                    userInfo.username = userData["username"].ToString();
                    UIManager.instance.mainUIScreen.Focus();
                }
                else if (password == userData["password"].ToString())
                {
                    userInfo.username = userData["username"].ToString();
                    UIManager.instance.mainUIScreen.Focus();
                }
            });
#else
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.GetDocument("Users", userId, gameObject.name, "OnLoginRequestSuccess", "");        
#endif

    }

    void OnLoginRequestSuccess(string jsonData)
    {
        Debug.LogError("LoginSuccess : " + jsonData);
        if (jsonData == "null")
            return;

        UserInfo respUserInfo = JsonUtility.FromJson<UserInfo>(jsonData);
        if (string.IsNullOrEmpty(userInfo.password))
        {
            userInfo.username = respUserInfo.username;
            UIManager.instance.mainUIScreen.Focus();
        }
        else if(userInfo.password == respUserInfo.password)
        {
            userInfo.username = respUserInfo.username;
            UIManager.instance.mainUIScreen.Focus();
        }
    }

    //Create a user document in firestore
    public void SinupUserByFireStore(string userId, string userName, string password)
    {

        UserInfo user = new UserInfo(userId, userName, password);
        
        Dictionary<string, System.Object> entryValues = user.ToDictionary();
#if !UNITY_WEBGL || UNITY_EDITOR
        mFirebaseFireStore.Collection("Users")
            .Document(userId)
            .SetAsync(entryValues);
        UIManager.instance.loginScreen.Focus();
#else
        string jsonData = UnityEngine.JsonUtility.ToJson(user);
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.SetDocument("Users", userId, jsonData , gameObject.name, "SignUpSuccess", "");        
#endif
    }

    public void SignUpSuccess(string output)
    {
        Debug.LogError("SignUpSuccess : " + output);
        UIManager.instance.loginScreen.Focus();       
    }

    //Save object info in firestore
    public void SaveObjectByFireStore(ObjectInfo objectInfo)
    {        
        Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object>();
        entryValues["objectId"] = objectInfo.objectId;
        entryValues["prefabName"] = objectInfo.prefabName;
        entryValues["objectType"] = objectInfo.objectType;
        entryValues["position"] = objectInfo.position;
        entryValues["rotation"] = objectInfo.rotation;
        entryValues["scale"] = objectInfo.scale;
        entryValues["posState"] = objectInfo.posState;
        entryValues["name"] = objectInfo.name;
        entryValues["description"] = objectInfo.description;
        entryValues["price"] = objectInfo.price;
        entryValues["webSiteUrl"] = objectInfo.webSiteUrl;
        entryValues["dataUrl"] = objectInfo.dataUrl;

        bool isDelete = false;
        if (string.IsNullOrEmpty(objectInfo.prefabName))
            isDelete = true;

#if !UNITY_WEBGL || UNITY_EDITOR
        if(isDelete)
        {
            mFirebaseFireStore.Collection("Users")
            .Document(userInfo.userId)
            .Collection("Objects")
            .Document(objectInfo.objectId)
            .DeleteAsync();
        }else
        {
            mFirebaseFireStore.Collection("Users")
            .Document(userInfo.userId)
            .Collection("Objects")
            .Document(objectInfo.objectId)
            .SetAsync(entryValues);
        }

#else
        string jsonData = UnityEngine.JsonUtility.ToJson(objectInfo);
        if(isDelete)
            FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.DeleteDocument("Users/" + userInfo.userId + "/Objects", objectInfo.objectId, gameObject.name, "", "");
        else
            FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.SetDocument("Users/" + userInfo.userId + "/Objects", objectInfo.objectId, isDelete ? null : jsonData, gameObject.name, "", "");
#endif

    }

    //Load csettings obj
    public void LoadCSettingInfo()
    {    

#if !UNITY_WEBGL || UNITY_EDITOR
    mFirebaseFireStore.Collection("Users")
            .Document(userInfo.userId)
            .Collection("csettings")
            .Document("csettings_doc")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                DocumentSnapshot documentSnapshot = task.Result;
                Dictionary<string, System.Object> csettingsDocument = documentSnapshot.ToDictionary();
                if (csettingsDocument == null)
                {
                    MainScreen.instance.LogOutButtonClicked();
                    return;
                }
                    
                if(csettingsDocument.ContainsKey("bgvolume"))
                    cSettingInfo.bgvolume = float.Parse(csettingsDocument["bgvolume"].ToString());
                cSettingInfo.bgsong = csettingsDocument["bgsong"].ToString();
                cSettingInfo.sfront = csettingsDocument["sfront"].ToString();
                cSettingInfo.sinterior = csettingsDocument["sinterior"].ToString();
                MainScreen.instance.InitCSettingObjects(cSettingInfo);

            });
#else
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.GetDocument("Users/" + userInfo.userId + "/csettings", "csettings_doc", gameObject.name, "OnLoadCSettingInfoRequestSuccess", "OnLoadCSettingInfoRequestFail");
#endif


    }

    public void OnLoadCSettingInfoRequestSuccess(string jsonData)
    {
        if (jsonData != "null")
        {
            cSettingInfo = JsonUtility.FromJson<CSettingInfo>(jsonData);
            MainScreen.instance.InitCSettingObjects(cSettingInfo);            
        }
    }

    public void OnLoadCSettingInfoRequestFail(string jsonData)
    {
        MainScreen.instance.LogOutButtonClicked();
    }
    //Load All Objects related to user in firestore
    public void LoadObjectsByFirestore()
    {
        List<ObjectInfo> objectList = new List<ObjectInfo>();
#if !UNITY_WEBGL || UNITY_EDITOR
        mFirebaseFireStore.Collection("Users")
            .Document(userInfo.userId)
            .Collection("Objects")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                QuerySnapshot querySnapShot = task.Result;

                foreach (DocumentSnapshot documentSnapshot in querySnapShot.Documents)
                {
                    Dictionary<string, System.Object> objData = documentSnapshot.ToDictionary();
                    ObjectInfo objectInfo = new ObjectInfo();
                    objectInfo.name = objData["name"].ToString();
                    objectInfo.description = objData["description"].ToString();
                    objectInfo.price = objData["price"].ToString();
                    objectInfo.webSiteUrl = objData["webSiteUrl"].ToString();
                    objectInfo.dataUrl = objData["dataUrl"].ToString();                    
                    objectInfo.objectId = documentSnapshot.Id;
                    objectInfo.objectType = (ObjectType)int.Parse(objData["objectType"].ToString());
                    objectInfo.prefabName = objData["prefabName"].ToString();
                    objectInfo.position = objData["position"].ToString();
                    objectInfo.rotation = objData["rotation"].ToString();
                    objectInfo.scale = objData["scale"].ToString();
                    objectInfo.posState = int.Parse(objData["posState"].ToString());

                    objectList.Add(objectInfo);
                }

                MainScreen.instance.InitObjects(objectList);

            });
#else
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.GetDocumentsInCollection("Users/" + userInfo.userId + "/Objects", gameObject.name, "OnLoadObjectRequestSuccess", "");        
#endif

    }

    public void OnLoadObjectRequestSuccess(string jsonData)
    {
        List<ObjectInfo> objectList = JsonConvert.DeserializeObject<List<ObjectInfo>>(jsonData);       
        MainScreen.instance.InitObjects(objectList);
    }

    public void LoadAccountNameListByFireStore()
    {
        List<string> accountNameList = new List<string>();
#if !UNITY_WEBGL || UNITY_EDITOR
        mFirebaseFireStore.Collection("Users")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                QuerySnapshot querySnapShot = task.Result;
                foreach(DocumentSnapshot documentSpnapShot in querySnapShot.Documents)
                {
                    accountNameList.Add(documentSpnapShot.Id);
                }

                AccountScreenForWeb.Instance.DisplayAccountNameList(accountNameList);
            });
#else
        FirebaseWebGL.Scripts.FirebaseBridge.FirebaseFirestore.GetDocumentsInCollection("Users", gameObject.name, "OnLoadAccountNameListRequestSuccess", "");
#endif

    }

    public void OnLoadAccountNameListRequestSuccess(string jsonData)
    {
        List<UserInfo> userList = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData);
        List<string> accountNameList = new List<string>();
        foreach(UserInfo userInfo in userList)
        {
            accountNameList.Add(userInfo.userId);
        }
        AccountScreenForWeb.Instance.DisplayAccountNameList(accountNameList);
    }

    public void OnDestroy()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(GameManager.gameStartState == GameStartState.None)
            mFirebaseFireStore.TerminateAsync();
        else if(GameManager.gameStartState == GameStartState.Logout)
            GameManager.gameStartState = GameStartState.None;
#endif
    }

    public void OnDisable()
    {
        //Debug.LogError("OnDisable");
        //mFirebaseFireStore.TerminateAsync();
    }
}

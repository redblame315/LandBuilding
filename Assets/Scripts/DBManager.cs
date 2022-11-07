using System.Collections;
using System.Collections.Generic;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Extensions;

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

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
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

    public ObjectInfo()
    { 
    }
    
}

public class ImageObjectInfo : ObjectInfo
{
    public string name;
    public string description;
    public float price = 0;
    public string webSiteUrl;
    public string imageUrl;
    public ImageObjectInfo()
    {

    }
}

public class VideoObjectInfo : ObjectInfo
{
    public string name;
    public string description;
    public float price = 0;
    public string webSiteUrl;
    public string videoUrl;
    public VideoObjectInfo()
    {

    }
}

public class DBManager
{
    static DBManager dbManager = null;
    DatabaseReference mDatabase;
    FirebaseDatabase mFireBaseDatabase;
    FirebaseFirestore mFirebaseFireStore;
    public UserInfo userInfo = new UserInfo();

    public static DBManager Instance()
    {
        if (dbManager == null)
            dbManager = new DBManager();

        return dbManager;
    }
    // Start is called before the first frame update
    public DBManager()
    {
        mFireBaseDatabase = FirebaseDatabase.GetInstance("https://landbuilding-5644c-default-rtdb.firebaseio.com");
        mFirebaseFireStore = FirebaseFirestore.DefaultInstance;
        mDatabase = mFireBaseDatabase.RootReference;
    }

    public void LoginUserByFireStore(string userId, string password)
    {
        mFirebaseFireStore.Collection("Users")
            .Document(userId)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task => {
                DocumentSnapshot snapshot = task.Result;
                Dictionary<string, object> userData = snapshot.ToDictionary();
                if(password == userData["password"].ToString())
                {
                    userInfo.userId = userId;
                    userInfo.username = userData["username"].ToString();
                    UIManager.instance.mainUIScreen.Focus();
                }
            });
    }

    public void LoginUser(string userId, string password)
    {
        mFireBaseDatabase
            .GetReference("Users").Child(userId)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Dictionary<string, Object> userData = (Dictionary<string, Object>)snapshot.GetValue(true);
                    if(password == userData["password"].ToString())
                    {
                        userInfo.userId = userId;
                        userInfo.username = userData["username"].ToString();
                        UIManager.instance.EnqueueAction(() => {
                            UIManager.instance.mainUIScreen.Focus();
                        });
                        
                    }
                }

            });
    }

    public void SinupUser(string userId, string userName, string password)
    {
        UserInfo user = new UserInfo(userId, userName, password);

        Dictionary<string, Object> entryValues = user.ToDictionary();
        Dictionary<string, Object> childUpdates = new Dictionary<string, Object>();
        childUpdates["Users/" + userId] = entryValues;
        mDatabase.UpdateChildrenAsync(childUpdates);

        UIManager.instance.loginScreen.Focus();
    }

    public void SinupUserByFireStore(string userId, string userName, string password)
    {
        UserInfo user = new UserInfo(userId, userName, password);

        Dictionary<string, Object> entryValues = user.ToDictionary();
        mFirebaseFireStore.Collection("Users")
            .Document(userId)
            .SetAsync(entryValues);

        UIManager.instance.loginScreen.Focus();
    }

    public void SaveObject(string objectId, string prefabName, string position, string rotation, string scale)
    {
        Dictionary<string, Object> entryValues = new Dictionary<string, Object>();
        entryValues["prefab_name"] = prefabName;
        entryValues["position"] = position;
        entryValues["rotation"] = rotation;
        entryValues["scale"] = scale;

        Dictionary<string, Object> childUpdates = new Dictionary<string, Object>();
        string key = "/Objects/" + userInfo.userId + "/" + objectId;
        childUpdates[key] = entryValues;
        mDatabase.UpdateChildrenAsync(childUpdates);
    }

    public void SaveObjectByFireStore(ObjectInfo objectInfo)
    {
        Dictionary<string, Object> entryValues = new Dictionary<string, Object>();
        entryValues["prefab_name"] = objectInfo.prefabName;
        entryValues["object_type"] = objectInfo.objectType;
        entryValues["position"] = objectInfo.position;
        entryValues["rotation"] = objectInfo.rotation;
        entryValues["scale"] = objectInfo.scale;

        if(objectInfo.objectType == ObjectType.Image)
        {
            ImageObjectInfo imageObjectInfo = (ImageObjectInfo)objectInfo;
            entryValues["name"] = imageObjectInfo.name;
            entryValues["description"] = imageObjectInfo.description;
            entryValues["price"] = imageObjectInfo.price;
            entryValues["website_url"] = imageObjectInfo.webSiteUrl;
            entryValues["image_url"] = imageObjectInfo.imageUrl;
        }else if(objectInfo.objectType == ObjectType.Video)
        {
            VideoObjectInfo videoObjectInfo = (VideoObjectInfo)objectInfo;
            entryValues["name"] = videoObjectInfo.name;
            entryValues["description"] = videoObjectInfo.description;
            entryValues["price"] = videoObjectInfo.price;
            entryValues["website_url"] = videoObjectInfo.webSiteUrl;
            entryValues["video_url"] = videoObjectInfo.videoUrl;
        }

        mFirebaseFireStore.Collection("Users")
            .Document(userInfo.userId)
            .Collection("Objects")
            .Document(objectInfo.objectId)
            .SetAsync(entryValues);

    }
    public void LoadObjectsByFirestore()
    {
        List<ObjectInfo> objectList = new List<ObjectInfo>();
        /*mFirebaseFireStore.Collection("Objects")
            .WhereEqualTo("user_id", userInfo.userId)
            .GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot querySnapShot = task.Result;
                foreach(DocumentSnapshot documentSnapshot in querySnapShot.Documents)
                {
                    Dictionary<string, Object> objData = documentSnapshot.ToDictionary();
                    ObjectInfo objectInfo = new ObjectInfo(objData["object_id"].ToString(), objData["prefab_name"].ToString(), objData["position"].ToString(), objData["rotation"].ToString(), objData["scale"].ToString());
                    objectList.Add(objectInfo);
                }

                MainScreen.instance.InitObjects(objectList);
            });*/

        mFirebaseFireStore.Collection("Users")
            .Document(userInfo.userId)
            .Collection("Objects")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                QuerySnapshot querySnapShot = task.Result;

                foreach (DocumentSnapshot documentSnapshot in querySnapShot.Documents)
                {
                    Dictionary<string, Object> objData = documentSnapshot.ToDictionary();
                    ObjectType objectType = (ObjectType)int.Parse(objData["object_type"].ToString());
                    ObjectInfo objectInfo;
                    switch(objectType)
                    {                        
                        case ObjectType.Image:
                            objectInfo = new ImageObjectInfo();
                            ImageObjectInfo imageObjectInfo = (ImageObjectInfo)objectInfo;
                            imageObjectInfo.name = objData["name"].ToString();
                            imageObjectInfo.description = objData["description"].ToString();
                            imageObjectInfo.price = float.Parse(objData["price"].ToString());
                            imageObjectInfo.webSiteUrl = objData["website_url"].ToString();
                            imageObjectInfo.imageUrl = objData["image_url"].ToString();
                            break;
                        case ObjectType.Video:
                            objectInfo = new VideoObjectInfo();
                            VideoObjectInfo vidoObjectInfo = (VideoObjectInfo)objectInfo;
                            vidoObjectInfo.name = objData["name"].ToString();
                            vidoObjectInfo.description = objData["description"].ToString();
                            vidoObjectInfo.price = float.Parse(objData["price"].ToString());
                            vidoObjectInfo.webSiteUrl = objData["website_url"].ToString();
                            vidoObjectInfo.videoUrl = objData["video_url"].ToString();
                            break;
                        default:
                            objectInfo = new ObjectInfo();
                            break;
                    }
                    objectInfo.objectId = documentSnapshot.Id;
                    objectInfo.objectType = (ObjectType)int.Parse(objData["object_type"].ToString());
                    objectInfo.prefabName = objData["prefab_name"].ToString();
                    objectInfo.position = objData["position"].ToString();
                    objectInfo.rotation = objData["rotation"].ToString();
                    objectInfo.scale = objData["scale"].ToString();                  

                    objectList.Add(objectInfo);
                }

                MainScreen.instance.InitObjects(objectList);

            });
    }

    public void LoadObjects()
    {
        List<ObjectInfo> objectList = new List<ObjectInfo>();
        mFireBaseDatabase
            .GetReference("Objects/" + userInfo.userId)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Dictionary<string, Object> objectsData = (Dictionary<string, Object>)snapshot.GetValue(true);
                    foreach (var obj in objectsData)
                    {
                        Dictionary<string, Object> objData = (Dictionary<string, Object>)obj.Value;
                        //ObjectInfo objectInfo = new ObjectInfo(obj.Key, objData["prefab_name"].ToString(), objData["position"].ToString(), objData["rotation"].ToString(), objData["scale"].ToString());
                        //objectList.Add(objectInfo);
                    }

                    UIManager.instance.EnqueueAction(() => {
                        MainScreen.instance.InitObjects(objectList);
                    });
                }

            });
    }
    /*public void WriteUserData(string username, string email)
    {        
        string key = mDatabase.Push().Key;
        UserInfo user = new UserInfo(username, email);

        Dictionary<string, Object> entryValues = user.ToDictionary();
        Dictionary<string, Object> childUpdates = new Dictionary<string, Object>();
        childUpdates["users/" + key] = entryValues;
        mDatabase.UpdateChildrenAsync(childUpdates);
    }*/

   /* public void RetrieveUserData()
    {
        List<UserInfo> userList = new List<UserInfo>();
        mFireBaseDatabase
            .GetReference("users")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                        
                }else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Dictionary<string, Object> usersData = (Dictionary<string, Object>)snapshot.GetValue(true);                    
                    foreach (var user in usersData)
                    {
                        Dictionary<string, Object> userData = (Dictionary<string, Object>)user.Value;
                        UserInfo tmpUser = new UserInfo(userData["username"].ToString(), userData["email"].ToString());
                        userList.Add(tmpUser);
                    }
                }

            });
    }*/
}

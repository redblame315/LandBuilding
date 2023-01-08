using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainScreen : UIScreen
{
    public static MainScreen instance = null;
    public Transform interiorDropSurface;
    public Transform frontDropSurface;
    
    [HideInInspector]
    public Transform frontSpawnPoint;
    public Transform frontInitSpawnPoint;
    public Transform interiorSpawnPoint;
    public Transform frontParentTransform;
    public Transform interiorParentTransform;
    
    public UILabel landTitleLabel;
    public TransformDialog normalObjectInfoDialog;
    public TransformDialog imageObjectInfoDialog;
    public TransformDialog videoObjectInfoDialog;
    
    public GuestImageDIalog guestImageDialog;
    public GuestVideoDIalog guestVideoDialog;
    public DescriptionDialog descriptionDialog;

    [HideInInspector]
    public TransformDialog curTransformDialog = null;
    [HideInInspector]
    public Dialog curGuestTransformDialog = null;
    public GameObject joyStickCanvas;
    public GameObject prefabScrollBar;
    public GameObject userInfoObj;
    public HeroCtrl heroCtrl;
    public InteriorEnterQuestionDialog enterQuationDialog;
    public InteriorExitQuestionDialog exitQuationDialog;
    public UISlider headBoneHeightSlider;
    public LoadingScript loadingAssetBundleUI;    
    [HideInInspector]
    public static AssetBundle assetBundle = null;
    [HideInInspector]
    public static string lastAssetBundleURI;
    [HideInInspector]
    public static bool bAssetBundleLoad = false;

    [DllImport("__Internal")]
    private static extern string GetURLFromPage();
    private void Awake()
    {
        instance = this;
    }

    //Init actions when the mainscreen is loaded
    public override void Init()
    {
        if(!GameManager.instance.forAdmin)
        {
            UserInfo userInfo = DBManager.userInfo;
            if(string.IsNullOrEmpty(userInfo.userId))
            {
                userInfo.userId = GameManager.instance.adminUserId;
                userInfo.username = GameManager.instance.adminUserName;
            }
        }
        //Show the user's info
        landTitleLabel.text = DBManager.userInfo.username;

        DBManager.Instance().LoadCSettingInfo();        
    }

    IEnumerator LoadAssetBundle(CSettingInfo cSettingInfo)
    {        
        string uri = cSettingInfo.sinterior;

#if UNITY_STANDALONE || UNITY_EDITOR
        //uri = "file:///" + Application.dataPath + "/AssetBundles/" + cSettingInfo.sinterior;
        uri = uri.Replace("/AssetBundles/", "/AssetBundles_PC/");
#elif UNITY_ANDROID
        uri = uri.Replace("/AssetBundles/", "/AssetBundles_Android/");
#elif UNITY_WEBGL
        /*uri = GetURLFromPage();
        if(uri.Contains("#"))
        {
            uri = uri.Split("#")[0];
        }
        uri += "/AssetBundles/" + cSettingInfo.sinterior;*/
        if (!GameManager.instance.forAdmin)
            uri = uri.Replace("admin", "guest");
#endif
        if(uri != lastAssetBundleURI)
        {
            bAssetBundleLoad = false;
            loadingAssetBundleUI.StartProgress();
            if (assetBundle != null)
                assetBundle.Unload(true);

            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);
            yield return request.SendWebRequest();
            assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            loadingAssetBundleUI.EndProgress();
            bAssetBundleLoad = true;

            lastAssetBundleURI = uri;
        }

        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCSettingObjects(CSettingInfo cSettingInfo)
    {
        StartCoroutine(LoadAssetBundle(cSettingInfo));

        //Load objects from firestore databse
        ProcessCSettingObjects(cSettingInfo);
    }

    public void InitRoad()
    {
        prefabScrollBar.SetActive(false);        

        frontParentTransform.gameObject.SetActive(false);
    }

    public void ProcessCSettingObjects(CSettingInfo cSettingInfo)
    {
        GameObject frontPrefab = Resources.Load("Prefabs/front/" + cSettingInfo.sfront) as GameObject;
        if (frontPrefab == null)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            LogOutButtonClicked();
#endif
            return;
        }

        GameObject[] frontGameObj = GameObject.FindGameObjectsWithTag("Front");
        for (int i = 0; i < frontGameObj.Length; i++)
            Destroy(frontGameObj[i]);

        GameObject frontObject = Instantiate(frontPrefab) as GameObject;
        frontObject.transform.parent = frontParentTransform;
        frontSpawnPoint = frontObject.transform.Find("SpawnPoint");

        frontParentTransform.gameObject.SetActive(true);

        HeroCtrl.instance.SpawnAtPoint(GameManager.gameStartState == GameStartState.ExitInterior ? frontSpawnPoint : frontInitSpawnPoint);
        if (GameManager.instance.forAdmin)
        {
            //headBoneHeightSlider.gameObject.SetActive(true);
            prefabScrollBar.SetActive(true);
            UIScrollView scrollView = prefabScrollBar.GetComponentInChildren<UIScrollView>();
            scrollView.ResetPosition();
            userInfoObj.SetActive(true);
        }
        else
        {
            joyStickCanvas.SetActive(true); //Show JoySticks for mobile
#if !UNITY_WEBGL || UNITY_EDITOR
            if (GameManager.instance.forAskAccountName)
                userInfoObj.SetActive(true);
#endif
        }

        DBManager.Instance().LoadObjectsByFirestore();
        GameManager.gameStartState = GameStartState.None;
    }

    //Load all objects with objectlist info from firestore.
    public void InitObjects(List<ObjectInfo> objectInfoList)
    {
        for(int i = 0; i < objectInfoList.Count; i++)
        {
            ObjectInfo objectInfo = objectInfoList[i];
            GameObject prefab = Resources.Load("Prefabs/" + objectInfo.prefabName) as GameObject;
            GameObject obj = Instantiate(prefab) as GameObject;
            obj.layer = LayerMask.NameToLayer("Object");
            obj.name = objectInfo.objectId;            
            obj.transform.parent = objectInfo.posState == ((int)HeroPosState.Front) ? frontDropSurface : interiorDropSurface;
            obj.transform.localPosition = JsonUtility.FromJson<Vector3>(objectInfo.position);
            obj.transform.localEulerAngles = JsonUtility.FromJson<Vector3>(objectInfo.rotation);
            obj.transform.localScale = JsonUtility.FromJson<Vector3>(objectInfo.scale);
            
            if(obj.tag == "ImageObject")
            {
                ImageObject imageObject = obj.GetComponent<ImageObject>();
                imageObject.InitImageObject(objectInfo);
            }else if(obj.tag == "VideoObject")
            {
                VideoObject videoObject = obj.GetComponent<VideoObject>();
                videoObject.InitVideoObject(objectInfo);
            }
        }

        //interiorParentTransform.gameObject.SetActive(false);
        GameManager.instance.bStart = true;
    }

    public void LogOutButtonClicked()
    {
        GameManager.gameStartState = GameStartState.Logout;
       
        DBManager.userInfo.userId = "";

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            InteriorManager.instance.ClearDontDestroyOnLoad();
        }

        SceneManager.LoadSceneAsync(0);        
    }

    public void HeadBoneHeightSliderChanged()
    {
        if(HeroCamera.instance)
        {
            HeroCamera.instance.ChangeHeadBonePosition(headBoneHeightSlider.value);
            UILabel heightLabel = headBoneHeightSlider.gameObject.GetComponentInChildren<UILabel>();
            heightLabel.text = string.Format("{0:0.00}", headBoneHeightSlider.value);
            UIManager.instance.bBusy = true;
        }
            
    }
    public void EnterInterior()
    {
        if (!bAssetBundleLoad)
            return;

        GameManager.instance.InitDontDestroyOnLoad();
        GameManager.gameStartState = GameStartState.EnterInterior;
        string[] scenePaths = assetBundle.GetAllScenePaths();
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);
        SceneManager.LoadScene(sceneName);
    }

    public void InitInterior()
    {
        HeroCtrl.instance.heroPosState = HeroPosState.Interior;
        HeroCtrl.instance.SpawnAtPoint(InteriorManager.instance.interiorSpawnPoint);

        //interiorParentTransform.gameObject.SetActive(true);
        PlayVideo(InteriorManager.instance.transform);

        //PlayVideo(frontParentTransform, false);
        frontParentTransform.gameObject.SetActive(false);
        interiorDropSurface.parent = InteriorManager.instance.interiorDropSurface;
        interiorDropSurface.transform.localPosition = Vector3.zero;

        
    }

    public void ExitInterior()
    {
        GameManager.gameStartState = GameStartState.ExitInterior;
        /*if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            interiorDropSurface.transform.parent = interiorParentTransform;
            interiorDropSurface.transform.localPosition = new Vector3(0, 1000, 0);
        }*/

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            InteriorManager.instance.ClearDontDestroyOnLoad();
        }
        SceneManager.LoadSceneAsync(0);
    }

    public void ResetFrontSettings()
    {
        Transform heroTransform = HeroCtrl.instance.characterController.transform;
        HeroCtrl.instance.characterController.enabled = false;
        heroTransform.position = frontSpawnPoint.position;
        heroTransform.rotation = frontSpawnPoint.rotation;
        HeroCtrl.instance.transform.rotation = heroTransform.rotation;
        HeroCtrl.instance.characterController.enabled = true;
        HeroCtrl.instance.heroPosState = HeroPosState.Interior;
        HeroCamera.instance.InitHeroCam();

        //PlayVideo(interiorParentTransform, false);
        //interiorParentTransform.gameObject.SetActive(false);

        frontParentTransform.gameObject.SetActive(true);
        PlayVideo(frontParentTransform);

        //SoundManager.instance.StopBackgroundSound();
    }

    public void PlayVideo(Transform parentTrans, bool isPlay = true)
    {
        VideoObject[] videoObjectArray = parentTrans.GetComponentsInChildren<VideoObject>();
        for (int i = 0; i < videoObjectArray.Length; i++)
            if(isPlay)
                videoObjectArray[i].videoPlayer.Play();
            else
                videoObjectArray[i].videoPlayer.Stop();
    }

    public void CloseCurDialog()
    {
        if (curGuestTransformDialog != null)
            curGuestTransformDialog.SetVisible(false);

        if (curTransformDialog != null) 
            curTransformDialog.SetVisible(false);
    }
}

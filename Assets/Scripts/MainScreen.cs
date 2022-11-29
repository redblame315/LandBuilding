using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : UIScreen
{
    public static MainScreen instance = null;
    [HideInInspector]
    public Transform interiorDropSurface;
    public Transform frontDropSurface;
    
    [HideInInspector]
    public Transform frontSpawnPoint;
    public Transform interiorSpawnPoint;
    public Transform frontParentTransform;
    public Transform interiorParentTransform;
    
    public UILabel landTitleLabel;
    public TransformDialog normalObjectInfoDialog;
    public TransformDialog imageObjectInfoDialog;
    public TransformDialog videoObjectInfoDialog;
    public GuestImageDIalog guestImageDialog;
    public GuestVideoDIalog guestVideoDialog;
    [HideInInspector]
    public TransformDialog curTransformDialog = null;
    public GameObject joyStickCanvas;
    public GameObject prefabScrollBar;
    public GameObject userInfoObj;
    public HeroCtrl heroCtrl;
    public InteriorEnterQuestionDialog enterQuationDialog;
    public InteriorExitQuestionDialog exitQuationDialog;
    public UISlider headBoneHeightSlider;
    

    private void Awake()
    {
        instance = this;
    }

    //Init actions when the mainscreen is loaded
    public override void Init()
    {
        if(!GameManager.instance.forAdmin)
        {
            UserInfo userInfo = DBManager.Instance().userInfo;
            if(string.IsNullOrEmpty(userInfo.userId))
            {
                userInfo.userId = GameManager.instance.adminUserId;
                userInfo.username = GameManager.instance.adminUserName;
            }
        }
        //Show the user's info
        landTitleLabel.text = DBManager.Instance().userInfo.username;
        //Load objects from firestore databse
        DBManager.Instance().LoadCSettingInfo();
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
        GameObject frontPrefab = Resources.Load("Prefabs/front/" + cSettingInfo.sfront) as GameObject;
        if (frontPrefab == null)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            LogOutButtonClicked();
#endif
            return;
        }            
        GameObject frontObject = Instantiate(frontPrefab) as GameObject;
        frontObject.transform.parent = frontParentTransform;
        frontSpawnPoint = frontObject.transform.Find("SpawnPoint");

        GameObject interiorPrefab = Resources.Load("Prefabs/interior/" + cSettingInfo.sinterior) as GameObject;
        if(interiorPrefab == null)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            LogOutButtonClicked();
#endif
            return;
        }
        GameObject interiorObject = Instantiate(interiorPrefab) as GameObject;
        interiorObject.transform.parent = interiorParentTransform;
        interiorDropSurface = interiorObject.transform.Find("DropSurface");
        interiorSpawnPoint = interiorObject.transform.Find("SpawnPoint");

        frontParentTransform.gameObject.SetActive(true);
        heroCtrl.gameObject.SetActive(true);
        HeroCamera.instance.InitHeroCam();        

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

        GameManager.instance.bStart = true;
    }

    public void LogOutButtonClicked()
    {
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
        Transform heroTransform = HeroCtrl.instance.characterController.transform;
        HeroCtrl.instance.characterController.enabled = false;        
        heroTransform.position = interiorSpawnPoint.position;
        heroTransform.rotation = interiorSpawnPoint.rotation;
        HeroCtrl.instance.transform.rotation = heroTransform.rotation;
        HeroCtrl.instance.characterController.enabled = true;
        HeroCtrl.instance.heroPosState = HeroPosState.Interior;
        HeroCamera.instance.InitHeroCam();

        interiorParentTransform.gameObject.SetActive(true);
        PlayVideo(interiorParentTransform);

        PlayVideo(frontParentTransform, false);
        frontParentTransform.gameObject.SetActive(false);      
        
        AudioClip backgroundAudioClip = Resources.Load("Audio/" + DBManager.Instance().cSettingInfo.bgsong) as AudioClip;
        SoundManager.instance.SetBackgroundVolume(DBManager.Instance().cSettingInfo.bgvolume);
        SoundManager.instance.PlayBackgroundSound(backgroundAudioClip);
    }

    public void ExitInterior()
    {
        Transform heroTransform = HeroCtrl.instance.characterController.transform;
        HeroCtrl.instance.characterController.enabled = false;
        heroTransform.position = frontSpawnPoint.position;
        heroTransform.rotation = frontSpawnPoint.rotation;
        HeroCtrl.instance.transform.rotation = heroTransform.rotation;
        HeroCtrl.instance.characterController.enabled = true;
        HeroCtrl.instance.heroPosState = HeroPosState.Interior;
        HeroCamera.instance.InitHeroCam();

        PlayVideo(interiorParentTransform, false);
        interiorParentTransform.gameObject.SetActive(false);

        frontParentTransform.gameObject.SetActive(true);
        PlayVideo(frontParentTransform);

        SoundManager.instance.StopBackgroundSound();
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
}

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
    public Transform interiorSpawnPoint;
    public Transform frontParentTransform;
    public Transform interiorParentTransform;
    
    public UILabel landTitleLabel;
    public TransformDialog normalObjectInfoDialog;
    public TransformDialog imageObjectInfoDialog;
    public TransformDialog videoObjectInfoDialog;
    public GameObject joyStickCanvas;
    public GameObject prefabScrollBar;
    public GameObject userInfoObj;
    public HeroCtrl heroCtrl;
    public InteriorEnterQuestionDialog enterQuationDialog;
    public UISlider headBoneHeightSlider;
    

    private void Awake()
    {
        instance = this;
    }

    //Init actions when the mainscreen is loaded
    public override void Init()
    {
        //Show the user's info
        landTitleLabel.text = DBManager.Instance().userInfo.username + "'s Land";
        //Show JoySticks for mobile
        joyStickCanvas.SetActive(true);

        if (GameManager.instance.forAdmin)
        {
            prefabScrollBar.SetActive(true);
            userInfoObj.SetActive(true);
        }

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
        GameObject frontObject = Instantiate(frontPrefab) as GameObject;
        frontObject.transform.parent = frontParentTransform;

        GameObject interiorPrefab = Resources.Load("Prefabs/interior/" + cSettingInfo.sinterior) as GameObject;
        GameObject interiorObject = Instantiate(interiorPrefab) as GameObject;
        interiorObject.transform.parent = interiorParentTransform;
        interiorDropSurface = interiorObject.transform.Find("DropSurface");
        interiorSpawnPoint = interiorObject.transform.Find("SpawnPoint");

        frontParentTransform.gameObject.SetActive(true);
        heroCtrl.gameObject.SetActive(true);
        HeroCamera.instance.InitHeroCam();        

        AudioClip backgroundAudioClip = Resources.Load("Audio/" + cSettingInfo.bgsong) as AudioClip;
        SoundManager.instance.PlayBackgroundSound(backgroundAudioClip);

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
        frontParentTransform.gameObject.SetActive(false);       
    }

}

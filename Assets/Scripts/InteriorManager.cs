using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorManager : MonoBehaviour
{
    public static InteriorManager instance = null;
    public Transform interiorSpawnPoint;
    public Transform interiorDropSurface;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameStartState = GameStartState.None;
        interiorSpawnPoint = transform.Find("SpawnPoint");
        interiorDropSurface = transform.Find("DropSurface");

        MainScreen.instance.InitInterior();

        //AudioClip backgroundAudioClip = Resources.Load("Audio/" + DBManager.cSettingInfo.bgsong) as AudioClip;
        string audioURI = DBManager.cSettingInfo.bgsong;
        if (!GameManager.instance.forAdmin)
            audioURI = audioURI.Replace("admin", "guest");

        /*GameObject soundmanagerObj = Instantiate(Resources.Load("Prefabs/SoundManager")) as GameObject;        
        soundmanagerObj.transform.parent = transform;

        Transform soundManagerSpawnTrans = transform.Find("SoundManager");
        soundmanagerObj.transform.localPosition = soundManagerSpawnTrans != null ? soundManagerSpawnTrans.localPosition : Vector3.zero;
        SoundManager soundManager = soundmanagerObj.GetComponent<SoundManager>();
        soundManager.Init();
        soundManager.PlayBackgroundSound(auidoURI);
        Debug.LogError("Volume: " + DBManager.cSettingInfo.bgvolume.ToString());*/
        SoundManager.instance.PlayBackgroundSound(audioURI);
        SoundManager.instance.SetBackgroundVolume(DBManager.cSettingInfo.bgvolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearDontDestroyOnLoad()
    {
        //Debug.LogError("ClearDontDestroyOnLoad : Begin");
        var go = new GameObject("go");
        DontDestroyOnLoad(go);

        foreach (var root in go.scene.GetRootGameObjects())
        {
            //Debug.LogError("DonDestroyObjects Name : " + root.name);
#if !UNITY_WEBGL || UNITY_EDITOR
            if (!root.name.Contains("DB") && !root.name.Contains("Fire") && !root.name.Contains("RealTime"))
                Destroy(root);
#else
            Destroy(root);
#endif
        }

        //Debug.LogError("ClearDontDestroyOnLoad : End");
    }
}

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearDontDestroyOnLoad()
    {
        Debug.LogError("ClearDontDestroyOnLoad : Begin");
        var go = new GameObject("go");
        DontDestroyOnLoad(go);

        foreach (var root in go.scene.GetRootGameObjects())
        {
            Debug.LogError("DonDestroyObjects Name : " + root.name);
#if !UNITY_WEBGL || UNITY_EDITOR
            if (!root.name.Contains("DB") && !root.name.Contains("Fire") && !root.name.Contains("RealTime"))
                Destroy(root);
#else
            Destroy(root);
#endif
        }

        Debug.LogError("ClearDontDestroyOnLoad : End");
    }
}

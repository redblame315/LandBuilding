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
        GameManager.instance.gameStartState = GameStartState.None;
        interiorSpawnPoint = transform.Find("SpawnPoint");
        interiorDropSurface = transform.Find("DropSurface");

        MainScreen.instance.InitInterior();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

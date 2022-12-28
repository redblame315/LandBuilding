using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour
{
    public Transform targetTransform;
    public UILabel progressLabel;
    public float fRotationSpeed = 5f;
    public int fProgress = 0;
    IEnumerator startRoutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTransform)
        {
            Vector3 eulerAngles = targetTransform.eulerAngles;
            targetTransform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z - fRotationSpeed * Time.deltaTime);
        }
        
    }

    public void StartProgress()
    {
        progressLabel.text = "0%";
        gameObject.SetActive(true);
        startRoutine = StartProgressRoutine();
        StartCoroutine(startRoutine);
    }

    public void EndProgress()
    {
        StopCoroutine(startRoutine);
        StartCoroutine(EndProgressRoutine());
    }

    IEnumerator StartProgressRoutine()
    {
        for(int i = 0; i < 93; i++)
        {
            progressLabel.text = string.Format("{0}%", i);
            yield return new WaitForSeconds(.5f);                 
        }    
    }

    IEnumerator EndProgressRoutine()
    {
        progressLabel.text = "100%";
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroPosState { Front, Interior}
public class HeroCtrl : MonoBehaviour
{
    public static HeroCtrl instance = null;
    public VariableJoystick joystick;
    public float fMoveSpeed = 3f;
    public CharacterController characterController;
    public HeroPosState heroPosState = HeroPosState.Front;
    Vector3 direction;
    Vector3 oldPosition;

    [HideInInspector]
    public Transform moveTargetTransform;
    public static bool isMovingTarget = false;
    Quaternion camRotation;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position;
        isMovingTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine move direction
        if (MainScreen.instance.curTransformDialog != null && MainScreen.instance.curTransformDialog.GetVisible())
            return;

        if (isMovingTarget)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal == 0 && vertical == 0)
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }

        direction = horizontal * transform.right + vertical * transform.forward;
        direction.Normalize();
        transform.position += direction * Time.deltaTime * fMoveSpeed;

        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 3, 1 << LayerMask.NameToLayer("Ground")))
        {
            GameObject hitObj = hit.collider.gameObject;
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1f))
        {
            MainScreen.instance.enterQuationDialog.SetVisible(hit.collider.tag == "Front");
            MainScreen.instance.exitQuationDialog.SetVisible(hit.collider.tag == "Interior");
        }
        else
        {
            MainScreen.instance.enterQuationDialog.SetVisible(false);
            MainScreen.instance.exitQuationDialog.SetVisible(false);
        }
    }

    private void LateUpdate()
    {
        if (MainScreen.instance.curTransformDialog != null && MainScreen.instance.curTransformDialog.GetVisible())
        {
            return;
        }
            

        if (isMovingTarget)
        {
            oldPosition = transform.position;
            return;
        }
            

        Vector3 dir = transform.position - oldPosition;
        //dir.Normalize();
        //Move the hero
        characterController.Move(dir);
        transform.position = characterController.transform.position;

        oldPosition = transform.position;
    }

    public void MoveToTarget(Transform targetTransform)
    {
        moveTargetTransform = targetTransform;

        RaycastHit hit;
        Vector3 targetPos = targetTransform.position;
        if(Physics.Raycast(targetPos + targetTransform.forward * 1f + Vector3.up, Vector3.down, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            targetPos = hit.point;
        }

        this.characterController.enabled = false;
        isMovingTarget = true;

        MainScreen.instance.CloseCurDialog();

        iTween.Stop();

        Hashtable args = new Hashtable();
        args.Add("position", targetPos);
        args.Add("looktarget", targetPos);
        args.Add("speed", 2f);
        args.Add("islocal", false);
        args.Add("onupdate", "OnMoveUpdate");
        args.Add("oncomplete", "OnMoveComplete");
        args.Add("easetype", iTween.EaseType.linear);
        iTween.MoveTo(gameObject, args);
    }

    public void OnMoveUpdate()
    {
        HeroCamera.instance.cam.LookAt(moveTargetTransform);
        /*Vector3 forward = moveTargetTransform.position - HeroCamera.instance.cam.transform.position;
        forward.Normalize();

        camRotation = Quaternion.LookRotation(forward);
        HeroCamera.instance.cam.rotation = Quaternion.LerpUnclamped(HeroCamera.instance.cam.rotation, camRotation, 50 * Time.deltaTime);
        //HeroCamera.instance.cam.rotation = camRotation;
        */
    }

    public void OnMoveComplete()
    {
        characterController.transform.position = transform.position;
        characterController.transform.rotation = transform.rotation;
        characterController.enabled = true;


        GameManager.instance.ShowTransformDialog(moveTargetTransform.gameObject);

        /*Hashtable args = new Hashtable();
        args.Add("rotation", camRotation.eulerAngles);
        args.Add("time", 1f);
        args.Add("easetype", iTween.EaseType.linear);
        args.Add("islocal", false);
        iTween.RotateTo(HeroCamera.instance.cam.gameObject, args);*/
        
        //isMovingTarget = false;
        StartCoroutine(InitCamAngleRoutine());
    }

    IEnumerator InitCamAngleRoutine()
    {
        yield return new WaitForSeconds(0f);
        HeroCamera.instance.InitAngle(HeroCamera.instance.cam.eulerAngles);
        isMovingTarget = false;
    }

    public void SpawnAtPoint(Transform targetPointTrans)
    {
        Transform heroTransform = characterController.transform;
        characterController.enabled = false;
        heroTransform.position = targetPointTrans.position;
        heroTransform.rotation = targetPointTrans.rotation;
        transform.rotation = heroTransform.rotation;
        characterController.enabled = true;
        HeroCamera.instance.InitHeroCam();
    }
}

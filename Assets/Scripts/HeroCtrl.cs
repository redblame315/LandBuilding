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

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Determine move direction
        if (MainScreen.instance.curTransformDialog != null && MainScreen.instance.curTransformDialog.GetVisible())
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
            return;

        Vector3 dir = transform.position - oldPosition;
        //dir.Normalize();
        //Move the hero
        characterController.Move(dir);
        transform.position = characterController.transform.position;

        oldPosition = transform.position;
    }
}

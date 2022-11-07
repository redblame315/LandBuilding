using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCtrl : MonoBehaviour
{
    public float fMoveSpeed = 3f;
    CharacterController characterController;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Determine move direction
        if (TransformDialog.instance.GetVisible())
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        direction = horizontal * transform.right + vertical * transform.forward;
        direction.Normalize();

        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    private void LateUpdate()
    {
        if (TransformDialog.instance.GetVisible())
            return;

        //Move the hero
        characterController.Move(direction * Time.deltaTime * fMoveSpeed);
    }
}

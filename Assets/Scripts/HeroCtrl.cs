using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCtrl : MonoBehaviour
{
    public float fMoveSpeed = 3f;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = horizontal * transform.right + vertical * transform.forward;
        direction.Normalize();
        characterController.Move(direction * Time.deltaTime * fMoveSpeed);   
    }
}

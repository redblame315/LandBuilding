using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HeroCtrl.instance == null)
            return;

        Vector3 direction = HeroCtrl.instance.transform.position - transform.position;
        direction.Normalize();
        Quaternion to = Quaternion.LookRotation(direction);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, to.eulerAngles.y, transform.eulerAngles.z);
    }
}

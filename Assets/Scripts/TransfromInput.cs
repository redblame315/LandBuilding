using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfromInput : MonoBehaviour
{
    UIInput input;
    // Start is called before the first frame update
    void Start()
    {
        input = gameObject.GetComponent<UIInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlusButtonClicked()
    {
        float preValue = float.Parse(input.value);
        preValue += .1f;
        input.value = preValue.ToString();
    }

    public void MinusButtonClicked()
    {
        float preValue = float.Parse(input.value);
        preValue -= .1f;
        input.value = preValue.ToString();
    }
}

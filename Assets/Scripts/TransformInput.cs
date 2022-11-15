using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TransformInput : MonoBehaviour
{
    InputField input;
    // Start is called before the first frame update
    void Start()
    {
        input = gameObject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlusButtonClicked()
    {
        float preValue = float.Parse(Utils.FloatString(input.text));
        preValue += .1f;
        input.text = preValue.ToString();
    }

    public void MinusButtonClicked()
    {
        float preValue = float.Parse(Utils.FloatString(input.text));
        preValue -= .1f;
        input.text = preValue.ToString();
    }
}

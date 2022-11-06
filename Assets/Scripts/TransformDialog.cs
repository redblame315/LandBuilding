using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformDialog : MonoBehaviour
{
    public static TransformDialog instance = null;
    public UIInput positionXInput;
    public UIInput positionYInput;
    public UIInput positionZInput;

    public UIInput rotationXInput;
    public UIInput rotationYInput;
    public UIInput rotationZInput;

    public UIInput scaleXInput;
    public UIInput scaleYInput;
    public UIInput scaleZInput;

    public bool isBusy = false;

    Transform targetTransform;
    string prefabName;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTarget(Transform _target, string _prefabName)
    {
        isBusy = true;
        targetTransform = _target;
        prefabName = _prefabName;

        positionXInput.text = targetTransform.localPosition.x.ToString();
        positionYInput.text = targetTransform.localPosition.y.ToString();
        positionZInput.text = targetTransform.localPosition.z.ToString();

        rotationXInput.text = targetTransform.localEulerAngles.x.ToString();
        rotationYInput.text = targetTransform.localEulerAngles.y.ToString();
        rotationZInput.text = targetTransform.localEulerAngles.z.ToString();

        scaleXInput.text = targetTransform.localScale.x.ToString();
        scaleYInput.text = targetTransform.localScale.y.ToString();
        scaleZInput.text = targetTransform.localScale.z.ToString();

        SetVisible(true);

        isBusy = false;
    }

    public void SetVisible(bool visible)
    {
        transform.localScale = visible ? Vector3.one : Vector3.zero;
    }

    public bool GetVisible()
    {
        return transform.localScale.x == 1;
    }
    public void SaveButtonClicked()
    {
        DBManager.Instance().SaveObjectByFireStore(targetTransform.name, prefabName, JsonUtility.ToJson(targetTransform.localPosition), JsonUtility.ToJson(targetTransform.localEulerAngles), JsonUtility.ToJson(targetTransform.localScale));
        SetVisible(false);
    }

    public void PositionValueChanged()
    {
        if (isBusy)
            return;

        float x = float.Parse(positionXInput.value);
        float y = float.Parse(positionYInput.value);
        float z = float.Parse(positionZInput.value);
        targetTransform.localPosition = new Vector3(x, y, z);
    }

    public void RotationValueChanged()
    {
        if (isBusy)
            return;

        float x = float.Parse(rotationXInput.value);
        float y = float.Parse(rotationYInput.value);
        float z = float.Parse(rotationZInput.value);
        targetTransform.localEulerAngles = new Vector3(x, y, z);
    }

    public void ScaleValueChanged()
    {
        if (isBusy)
            return;

        float x = float.Parse(scaleXInput.value);
        float y = float.Parse(scaleYInput.value);
        float z = float.Parse(scaleZInput.value);
        targetTransform.localScale = new Vector3(x, y, z);
    }
}

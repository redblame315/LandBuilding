using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionDialog : Dialog
{
    public UILabel nameLabel;
    public UILabel descriptionLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string _name, string _description)
    {        
        nameLabel.text = _name;
        descriptionLabel.text = _description;

        transform.localPosition =  UIManager.convertMousePos2NGUIPos(Input.mousePosition);
        
        SetVisible(true);
    }
}

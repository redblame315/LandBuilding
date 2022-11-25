using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountNameItem : MonoBehaviour
{
    public UILabel nameLabel;
    public static string selectedName = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAccountName(string accountName)
    {
        nameLabel.text = accountName;
    }

    public void ItemClicked()
    {
        selectedName = nameLabel.text;
    }
}

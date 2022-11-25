using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountScreenForWeb : UIScreen
{
    public static AccountScreenForWeb Instance = null;
    UIGrid nameListGrid;
    UIScrollBar scrollBar;
    private void Awake()
    {
        Instance = this;
    }
    public override void Init()
    {
        nameListGrid = gameObject.GetComponentInChildren<UIGrid>();
        scrollBar = gameObject.GetComponentInChildren<UIScrollBar>();
        DBManager.Instance().LoadAccountNameListByFireStore();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OkButtonClicked()
    {
        if (string.IsNullOrEmpty(AccountNameItem.selectedName))
            return;

        DBManager.Instance().LoginUserByFireStore(AccountNameItem.selectedName, "");
    }

    public void DisplayAccountNameList(List<string> accountNameList)
    {
        foreach(string accountName in accountNameList)
        {
            GameObject item = Instantiate(Resources.Load("Prefabs/AccountNameItem")) as GameObject;
            AccountNameItem accountNameItem = item.GetComponent<AccountNameItem>();
            accountNameItem.SetAccountName(accountName);
            item.transform.parent = nameListGrid.transform;
            item.transform.localScale = Vector3.one;
        }

        nameListGrid.Reposition();
        StartCoroutine(InitScrollValue());
    }

    IEnumerator InitScrollValue()
    {
        yield return new WaitForSeconds(.3f);
        scrollBar.value = 0.1f;
        yield return new WaitForSeconds(.3f);
        scrollBar.value = 0;
    }
}

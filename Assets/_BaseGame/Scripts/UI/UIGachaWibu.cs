using System.Collections;
using System.Collections.Generic;
using TW.UI.CustomComponent;
using UnityEngine;

public class UIGachaWibu : AUIPanel
{
    [field: SerializeField] public UICharacterInfo UICharacter {get; private set;}
    [field: SerializeField] public AUIButton ScoutButton {get; private set;}
    [field: SerializeField] public AUIButton BigScoutButton {get; private set;}
    [field: SerializeField] public AUIButton ButtonClose {get; private set;}
    
    protected override void Awake()
    {
        base.Awake();
        ScoutButton.OnClickButton.AddListener(OnClickButtonScout);
        BigScoutButton.OnClickButton.AddListener(OnClickButtonBigScout);
        ButtonClose.OnClickButton.AddListener(OnClickButtonClose);
    }

    public void OnClickButtonScout()
    {
        Debug.Log("Scout");
    }
    public void OnClickButtonBigScout()
    {
        Debug.Log("BigScout");
    }

    public void OnClickButtonClose()
    {
        ClosePanel<UIGachaWibu>();
    }
}

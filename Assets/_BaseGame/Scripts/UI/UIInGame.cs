using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using TW.UI.CustomComponent;
using TW.Utility.CustomComponent;
using UnityEngine;

public class UIInGame : AUIPanel
{   
    [field: SerializeField] private AUIButton HomeButton {get; set;}
    [field: SerializeField] public UIHeroInGame[] UIHeroInGame {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        HomeButton.OnClickButton.AddListener(OnClickHomeButton);
    }

    public void SetupOnOpen(List<CharacterConfig> characterConfigs)
    {
        characterConfigs.ForEach((cf, i) =>
        {
            UIHeroInGame[i].Setup(cf);
        });
    }

    private void OnClickHomeButton()
    {
        // TODO: Open Home Panel
        //GameManager.Instance.SetGameState(GameManager.GameState.SelectLevel);
        AUIManager.Instance.CloseUI<UIInGame>();
        GameManager.Instance.ClearAllMap();
        AUIManager.Instance.OpenUI<BaseGame.PopupMapUI>();
    }
}

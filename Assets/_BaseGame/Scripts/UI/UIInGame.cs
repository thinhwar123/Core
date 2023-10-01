using System.Collections;
using System.Collections.Generic;
using BaseGame;
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
        AudioManager.Instance.ChangeMusic(AudioType.BgmGamePlay, 1);
    }


    private void OnClickHomeButton()
    {
        // TODO: Open Home Panel
        AudioManager.Instance.ChangeMusic(AudioType.BgmLevelSelect, 1);
        GameManager.Instance.SetGameState(GameManager.GameState.SelectLevel);
        GameManager.Instance. ClearAllMap();
        AUIManager.Instance.CloseUI<UIInGame>();
        AUIManager.Instance.CloseUI<UIResult>();
        AUIManager.Instance.OpenUI<PopupMapUI>();
    }
}

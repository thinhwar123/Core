using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TW.UI.CustomComponent;
using TW.Utility.CustomComponent;
using BaseGame;

public class UIResult : AUIPanel
{
    [field: SerializeField] private GameObject LoseObject;
    [SerializeField] private GameObject WinObject;
    [field: SerializeField] private AUIButton HomeButton {get; set;}
    [field: SerializeField] private AUIButton ReplayButton {get; set;}
    [field: SerializeField] private AUIButton NextButton {get; set;}
    protected override void Awake()
    {
        base.Awake();
        HomeButton.OnClickButton.AddListener(OnClickHomeButton);
        ReplayButton.OnClickButton.AddListener(()=>{
            GameManager.Instance.OnReplay();
            AUIManager.Instance.OpenUI<UIInGame>();
            AUIManager.Instance.CloseUI<UIResult>();
            });
        NextButton.OnClickButton.AddListener(()=>{
            AUIManager.Instance.CloseUI<UIInGame>();
            AUIManager.Instance.CloseUI<UIResult>();
            GameManager.Instance.ClearAllMap();
            AUIManager.Instance.OpenUI<BaseGame.PopupMapUI>();
        });
    }
    public void SetupOnOpen(bool isWon)
    {
        LoseObject.SetActive(!isWon);
        WinObject.SetActive(isWon);
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

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TW.UI.CustomComponent;
using UnityEngine;

public class UIGachaWibu : AUIPanel
{
    [field: SerializeField] public UICharacterInfo UICharacter {get; private set;}
    [field: SerializeField] public AUIButton ScoutButton {get; private set;}
    [field: SerializeField] public AUIButton BigScoutButton {get; private set;}
    [field: SerializeField] public AUIButton ButtonClose {get; private set;}
    [field: SerializeField] public Transform StartPosition {get; private set;}
    [field: SerializeField] public Transform EndPosition {get; private set;}
    protected override void Awake()
    {
        base.Awake();
        ScoutButton.OnClickButton.AddListener(OnClickButtonScout);
        BigScoutButton.OnClickButton.AddListener(OnClickButtonBigScout);
        ButtonClose.OnClickButton.AddListener(OnClickButtonClose);
    }
    public void SetupOnOpen()
    {
        AudioManager.Instance.ChangeMusic(AudioType.BgmGacha, 1);
    }

    public void OnClickButtonScout()
    {
        Debug.Log("Scout");
        CharacterConfig characterConfig = GachaGlobalConfig.Instance.GetRandomCharacterConfig();
        UICharacterInfo ui = Instantiate(UICharacter, StartPosition.position, Quaternion.identity, Transform);
        ui.Setup(characterConfig);

        ui.transform.DOMove(EndPosition.position, 0.75f);
    }
    public void OnClickButtonBigScout()
    {
        Debug.Log("BigScout");
        for (int i = 0; i < 11; i++)
        {
            CharacterConfig characterConfig = GachaGlobalConfig.Instance.GetRandomCharacterConfig();
            UICharacterInfo ui = Instantiate(UICharacter, StartPosition.position, Quaternion.identity, Transform);
            ui.Setup(characterConfig);

            ui.transform.DOMove(EndPosition.position, 0.75f).SetDelay(i * 0.3f);
        }

    }

    public void OnClickButtonClose()
    {
        ClosePanel<UIGachaWibu>();
        AudioManager.Instance.ChangeMusic(AudioType.BgmLevelSelect, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TW.UI.CustomComponent;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : MonoBehaviour
{
    [field: SerializeField] public CharacterConfig CharacterConfig {get; private set;}
    [field: SerializeField] public Image ImagePreview {get; private set;}
    [field: SerializeField] public AUIText TextCharacterName {get; private set;}
    [field: SerializeField] public AUIText TextNickName {get; private set;}
    [field: SerializeField] public GameObject[] CharacterStar {get; private set;}
    [field: SerializeField] public AUIText TextProfile {get; private set;}
    [field: SerializeField] public AUIText TextAttack {get; private set;}
    [field: SerializeField] public AUIText TextHp {get; private set;}

    [Button]
    public void Setup(CharacterConfig characterConfig)
    {
        CharacterConfig = characterConfig;
        ImagePreview.sprite = CharacterConfig.CharacterPreview;
        TextCharacterName.Text = CharacterConfig.CharacterName;
        TextNickName.Text = CharacterConfig.NickName;
        TextProfile.Text = CharacterConfig.Profile;
        TextAttack.Text = CharacterConfig.AttackDamage.ToString();
        TextHp.Text = CharacterConfig.HitPoint.ToString();
        
        for (int i = 0; i < CharacterStar.Length; i++)
        {
            CharacterStar[i].SetActive(i < CharacterConfig.CharacterStar);
        }
    }
}

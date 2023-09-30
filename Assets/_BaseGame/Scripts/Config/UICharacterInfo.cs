using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.UI.CustomComponent;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UICharacterInfo : MonoBehaviour
{
    [field: SerializeField] public CharacterConfig CharacterConfig {get; private set;}
    [field: SerializeField] public Image ImagePreview {get; private set;}
    [field: SerializeField] public Image ImageSkill {get; private set;}
    [field: SerializeField] public AUIText TextCharacterName {get; private set;}
    [field: SerializeField] public AUIText TextNickName {get; private set;}
    [field: SerializeField] public GameObject[] CharacterStar {get; private set;}
    [field: SerializeField] public AUIText TextProfile {get; private set;}
    [field: SerializeField] public AUIText TextAttack {get; private set;}
    [field: SerializeField] public AUIText TextHp {get; private set;}
    [field: SerializeField] public AUIButton ButtonClose {get; private set;}
    [field: SerializeField] public string[] ColorConfigs {get; private set;}
    private void Awake()
    {
        ButtonClose.OnClickButton.AddListener(OnClickButtonClose);
    }

    [Button]
    public void Setup(CharacterConfig characterConfig)
    {
        CharacterConfig = characterConfig;
        ImagePreview.sprite = CharacterConfig.CharacterPreview;
        ImageSkill.sprite = CharacterConfig.SkillConfig.SkillIcon;
        TextCharacterName.Text = CharacterConfig.CharacterName;
        TextNickName.Text = $"<color=#{ColorConfigs[(int)CharacterConfig.CharacterAttribute]}>{CharacterConfig.NickName}";
        TextProfile.Text = CharacterConfig.Profile;
        TextAttack.Text = CharacterConfig.AttackDamage.ToString();
        TextHp.Text = CharacterConfig.HitPoint.ToString();
        
        for (int i = 0; i < CharacterStar.Length; i++)
        {
            CharacterStar[i].SetActive(i < CharacterConfig.CharacterStar);
        }
        
    }
    private void OnClickButtonClose()
    {
        Vector2 random = Random.insideUnitCircle.normalized * 2000;
        transform.DOMove(random, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}

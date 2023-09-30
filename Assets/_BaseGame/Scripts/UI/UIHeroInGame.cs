
using Sirenix.Utilities;
using TW.UI.CustomComponent;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroInGame : MonoBehaviour
{
    [field: SerializeField] public CharacterConfig CharacterConfig {get; private set;}
    [field: SerializeField] public GameObject[] HeroAttribute {get; private set;}
    [field: SerializeField] public Image ImageHero {get; private set;}
    [field: SerializeField] public AUIButton MainButton {get; private set;}

    private void Awake()
    {
        MainButton.OnClickButton.AddListener(OnClickButtonMain);
    }
    [Button]
    public void Setup(CharacterConfig characterConfig)
    {
        CharacterConfig = characterConfig;
        ImageHero.sprite = characterConfig.CharacterIcon;
        HeroAttribute.ForEach((x, i) => x.SetActive((int)CharacterConfig.CharacterAttribute == i));
        
    }

    private void OnClickButtonMain()
    {
        
    }
}

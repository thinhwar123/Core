using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using UnityEngine;

public partial class Character : Entity
{
    [field: SerializeField] public CharacterConfig CharacterConfig {get; private set;}
    [field: SerializeField] public Transform CharacterModelContainer {get; private set;}
    [field: SerializeField] private CharacterModel CharacterModel {get; set;}
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public string CurrentAnimation {get; private set;}

    public virtual void Init(CharacterConfig characterConfig)
    {
        CharacterConfig = characterConfig;
        CharacterModel = Instantiate(CharacterConfig.CharacterModel, CharacterModelContainer);
        Animator = CharacterModel.GetComponent<Animator>();
    }
    public virtual void PlayAnimation(string animationName)
    {
        if (animationName == "") return;
        if (CurrentAnimation == animationName) return;
        Animator.Play(animationName);
    }
}

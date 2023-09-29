using System;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using UnityEngine;
using UnityEngine.UI;


public class UIHealthBar : AwaitableCachedMonoBehaviour
{
    [field: SerializeField] public Color[] Colors { get; private set; }
    [field: SerializeField] public Image ImageAttributeType {get; private set;}
    [field: SerializeField] public Slider ProcessBar {get; private set;}
    [field: SerializeField] public Vector3 Offset {get; private set;}
    [field: SerializeField] public Transform Target {get; private set;}
    [field: SerializeField] public Image ImageFill {get; private set;}
    [field: SerializeField] public Color EnemyColor {get; private set;}
    public void SetupHealthBar(EAttribute attributeType, Transform target)
    {
        if (attributeType == EAttribute.White)
        {
            ImageAttributeType.color = Color.clear;
            ImageFill.color = EnemyColor;
        }
        else
        {
            ImageAttributeType.color = Colors[(int)attributeType];
        }
        Target = target;

    }
    
    public void Update()
    {
        if (Target == null) return;
        Transform.position =  CameraManager.Instance.MainCamera.WorldToScreenPoint(Target.position + Offset);
    }
    
    public void UpdateValue(float value)
    {
        ProcessBar.value = value;
    }
}

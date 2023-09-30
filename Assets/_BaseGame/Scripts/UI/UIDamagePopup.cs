using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TW.UI.CustomComponent;
using TW.Utility.CustomComponent;
using UnityEngine;

public class UIDamagePopup : AwaitableCachedMonoBehaviour
{
    [field: SerializeField] public AUIText TextDamage {get; private set;}
    [field: SerializeField] public Vector3 Offset {get; private set;}
    [field: SerializeField] public Transform Target {get; private set;}
    
    public void SetupDamagePopup(int damage, Transform target)
    {
        Target = target;
        TextDamage.Text = damage.ToString();
        Transform.position =  CameraManager.Instance.MainCamera.WorldToScreenPoint(Target.position + Offset);
        Transform.DOMoveY(Transform.position.y + 50, 1f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}

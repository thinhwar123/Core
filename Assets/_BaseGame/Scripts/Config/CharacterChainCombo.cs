using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public partial class Character
{
    [System.Serializable]
    public class ChainCombo
    {
       
        [field: SerializeField] public int ChainCount {get; private set;}
        [field: SerializeField, InlineEditor] public ChainComboConfig ChainComboConfig { get; set; }
        [ShowInInspector] public EAreaType ChainComboType => ChainComboConfig?.ChangeComboType ?? EAreaType.None;
    }
}

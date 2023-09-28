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
        public enum Type
        {
            None = 0,
            SmallCross = 1,
            SmallXMark = 2,
            SmallLineCross = 3,
            SmallArea = 4,
        
            LineCross = 10,
            XMark = 11,
            LineXMark = 12,
            Area = 13,
            
            GlobalArea = 20,
        }
        
        [field: SerializeField] public int ChainCount {get; private set;}
        [field: SerializeField, InlineEditor] public ChainComboConfig ChainComboConfig { get; set; }
        [ShowInInspector] public Type ChainComboType => ChainComboConfig?.ChangeComboType ?? Type.None;
    }
}

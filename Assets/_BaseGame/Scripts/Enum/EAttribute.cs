using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[GUIColor("@TW.Utility.Extension.AColorExtension.GetColorInPalette(\"Attribute\", (int)$value)")]
public enum EAttribute 
{
    
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3,
    
    White = 4,
    Grey = 5,
    
    Clear = 10,
}
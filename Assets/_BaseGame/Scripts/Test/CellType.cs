using System;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.Extension;
using UnityEngine;

public partial class Cell
{
    [Flags]
    public enum Type
    {
        None = 0,
        
        Red = 1 << 0,
        Green = 1 << 1,
        Blue = 1 << 2,
        Yellow = 1 << 3,
        
        All = Red | Green | Blue | Yellow,
    }
    private static Type[] BasicTypes { get; set; } = new Type[] {Type.Red, Type.Green, Type.Blue, Type.Yellow};

    public static Type GetRandomBasicType()
    {
        return BasicTypes.GetRandomElement();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.Extension;
using UnityEngine;

public partial class Cell
{
    private static EAttribute[] BasicTypes { get; set; } = new EAttribute[] {EAttribute.Red, EAttribute.Green, EAttribute.Blue, EAttribute.Yellow};

    public static EAttribute GetRandomBasicType()
    {
        return BasicTypes.GetRandomElement();
    }
}

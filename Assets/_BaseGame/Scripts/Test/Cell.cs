using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TW.Utility.CustomComponent;
using UnityEngine;

public partial class Cell : AwaitableCachedMonoBehaviour
{
    [field: SerializeField] public Config[] CellConfigs {get; private set;}
    [field: SerializeField] public Type CellType {get; private set;}
    
    public void SetupCell(Type type)
    {
        CellType = type;
        Config config = CellConfigs.FirstOrDefault(c => c.CellType == type);
        config.NormalCell.SetActive(true);
        config.SelectedCell.SetActive(false);
        config.UnSelectCell.SetActive(false);
    }
    
}
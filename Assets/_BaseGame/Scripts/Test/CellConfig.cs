using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Cell
{
    [System.Serializable]
    public class Config
    {
        [field: SerializeField] public Type CellType {get; private set;}
        [field: SerializeField] public Color CellColor {get; private set;}
        [field: SerializeField] public GameObject NormalCell {get; private set;}
        [field: SerializeField] public GameObject SelectedCell {get; private set;}
        [field: SerializeField] public GameObject UnSelectCell {get; private set;}
    }
    
}

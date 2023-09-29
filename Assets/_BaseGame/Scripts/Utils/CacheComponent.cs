using System.Collections.Generic;
using UnityEngine;

public class CacheComponent 
{
    private static Dictionary<Collider, Cell> CellDictionary { get; set; } = new Dictionary<Collider, Cell>();
    public static Cell GetCell(Collider collider)
    {
        if (!CellDictionary.ContainsKey(collider))
        {
            CellDictionary.Add(collider, collider.GetComponent<Cell>());
        }
        return CellDictionary[collider];
    }
}
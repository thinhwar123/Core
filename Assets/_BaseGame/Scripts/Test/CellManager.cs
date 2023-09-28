using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [field: SerializeField] public Cell CellPrefab {get; private set;}
    [field: SerializeField] public int Column {get; private set;}
    [field: SerializeField] public int Row {get; private set;}
    [field: SerializeField] public List<Cell> CellList {get; private set;} = new List<Cell>();
    private Cell[,] Cells {get; set;}
    
    private void CreateMap()
    {
        Cells = new Cell[Column, Row];
        for (int i = 0; i < Column; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                Cells[i, j] = Instantiate(CellPrefab, transform);
                Cells[i, j].transform.localPosition = new Vector3(i, j, 0);
            }
        }
    }
#if UNITY_EDITOR
    [Button]
    private void InitDemoMap()
    {
        CellList.ForEach(x => DestroyImmediate(x.gameObject));
        CellList.Clear();
        
        Cells = new Cell[Column, Row];
        for (int i = 0; i < Column; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                Cells[i, j] = UnityEditor.PrefabUtility.InstantiatePrefab(CellPrefab, transform) as Cell;
                Cells[i, j].transform.localPosition = new Vector3(i - Column/2f, 0, j - Row/2f);
                Cells[i, j].SetupCell(Cell.GetRandomBasicType());
                
                CellList.Add(Cells[i, j]);
            }
        }
    }
#endif
}

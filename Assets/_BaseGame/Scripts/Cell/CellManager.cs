using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TW.Utility.DesignPattern;
using UnityEngine;

public class CellManager : Singleton<CellManager>
{
    [field: SerializeField] public Cell CellPrefab {get; private set;}
    [field: SerializeField] public int Column {get; private set;}
    [field: SerializeField] public int Row {get; private set;}
    [field: SerializeField] public int Stroke {get; private set;}
    [field: SerializeField] public List<Cell> CellList {get; private set;} = new List<Cell>();
    
    private Cell[,] Cells {get; set;}

    private void Start()
    {
        CreateMap();
    }

    private void CreateMap()
    {
        ClearDemoMap();
        Cells = new Cell[Column, Row];
        for (int i = 0; i < Column; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                Cells[i, j] = Instantiate(CellPrefab, Transform);
                Cells[i, j].transform.localPosition = new Vector3(i - Column/2f, 0, j - Row/2f);
                Cells[i, j].SetupCell(Cell.GetRandomBasicType(), i, j);
                
                CellList.Add(Cells[i, j]);
                if (i + j < Stroke || Column - i + j < Stroke + 1 || i + Row - j < Stroke + 1 || Column - i + Row - j < Stroke + 2)
                {
                    Cells[i, j].SetupHide();
                }
            }
        }
    }

    private void ClearMap()
    {
        CellList.ForEach(x => Destroy(x.gameObject));
        CellList.Clear();
    }
    
    
    private Cell GetCell(int x, int y)
    {
        if (x < 0 || x >= Column || y < 0 || y >= Row) return null; 
        return Cells[x, y];
    }

    private List<Cell> GetCellAround(int x, int y, int range)
    {
        List<Cell> result = new List<Cell>();
        for (int i = x - range; i <= x + range; i++)
        {
            for (int j = y - range; j <= y + range; j++)
            {
                if (i == x && j == y) continue;
                Cell cell = GetCell(i, j);
                if (cell == null) continue;
                result.Add(cell);
            }
        }
        return result;
    }

    private List<Cell> GetCellCross(int x, int y, int range)
    {
        List<Cell> result = new List<Cell>();
        for (int i = x - range; i <= x + range; i++)
        {
            if (i == y) continue;
            Cell cell = GetCell(i, y);
            if (cell == null) continue;
            result.Add(cell);
        }
        for (int j = y - range; j <= y + range; j++)
        {
            if (x == j) continue;
            Cell cell = GetCell(x, j);
            if (cell == null) continue;
            result.Add(cell);
        }
        return result;
    }

    private List<Cell> GetCellXMark(int x, int y, int range)
    {
        List<Cell> result = new List<Cell>();
        for (int i = x - range; i <= x + range; i++)
        {
            for (int j = y - range; j <= y + range; j++)
            {
                if (i == x && j == y) continue;
                if (Mathf.Abs(i - x) == Mathf.Abs(j - y))
                {
                    Cell cell = GetCell(i, j);
                    if (cell == null) continue;
                    result.Add(cell);
                }
            }
        }
        return result;
    }

    public List<Cell> GetCell(EAreaType areaType, int x, int y)
    {
        List<Cell> result = new List<Cell>();
        switch (areaType)
        {
            case EAreaType.None:
                break;
            case EAreaType.SmallCross:
                result = GetCellCross(x, y, 1);
                break;
            case EAreaType.SmallXMark:
                result = GetCellXMark(x, y, 1);
                break;
            case EAreaType.SmallLineCross:
                result = GetCellCross(x, y, 2);
                break;
            case EAreaType.SmallArea:
                result = GetCellAround(x, y, 1);
                break;
            case EAreaType.LineCross:
                result = GetCellCross(x, y, 10);
                break;
            case EAreaType.XMark:
                result = GetCellXMark(x, y, 10);
                break;
            case EAreaType.LineXMark:
                result = GetCellCross(x, y, 10);
                result.AddRange(GetCellCross(x, y, 10));
                break;
            case EAreaType.Area:
                result = GetCellAround(x, y, 3);
                break;
            case EAreaType.GlobalArea:
                result = CellList;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(areaType), areaType, null);
        }
        //remove duplicate elements and input elements and return
        return result.Distinct().ToList();
    }
    
    public void DeActiveAllOtherCell(EAttribute selectAttribute)
    {
        CellList.Where(x => x.CellAttribute != selectAttribute).ForEach(x => x.SetupUnSelect());
    }
    public void FocusAllCell(EAttribute selectAttribute)
    {
        CellList.Where(x => x.CellAttribute == selectAttribute).ForEach(x => x.SetupFocus());
    }
    public void NormalAllCell()
    {
        CellList.ForEach(x => x.SetupNormal());
    }
    
#if UNITY_EDITOR
    [Button]
    private void InitDemoMap()
    {
        ClearDemoMap();
        Cells = new Cell[Column, Row];
        for (int i = 0; i < Column; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                Cells[i, j] = UnityEditor.PrefabUtility.InstantiatePrefab(CellPrefab, transform) as Cell;
                Cells[i, j].transform.localPosition = new Vector3(i - Column/2f, 0, j - Row/2f);
                Cells[i, j].SetupCell(Cell.GetRandomBasicType(), i, j);
                
                CellList.Add(Cells[i, j]);
                if (i + j < Stroke || Column - i + j < Stroke + 1 || i + Row - j < Stroke + 1 || Column - i + Row - j < Stroke + 2)
                {
                    Cells[i, j].SetupHide();
                }
            }
        }
    }
    [Button]
    private void InitDemoMapAndActiveArea(EAreaType areaType)
    {
        InitDemoMap();
        List<Cell> selectCells = GetCell(areaType, Column/2, Row/2);
        selectCells.ForEach(x => x.SetupSelect());
        CellList.Except(selectCells).ToList().ForEach(x => x.SetupUnSelect());
    }

    [Button]
    private void ClearDemoMap()
    {
        CellList.ForEach(x => DestroyImmediate(x.gameObject));
        CellList.Clear();
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TW.Utility.CustomComponent;
using UnityEngine;

public partial class Cell : AwaitableCachedMonoBehaviour
{
    public enum State
    {
        Normal,
        Selected,
        UnSelect,
        Focus,
        Hide,
    }
    [field: SerializeField] public Config[] CellConfigs {get; private set;}
    [field: SerializeField] public State CurrentState {get; private set;}
    [field: SerializeField] public GameObject LineSelect {get; private set;}
    [field: SerializeField] public GameObject LineUnSelect {get; private set;}
    [field: SerializeField] public Entity Owner {get; private set;}
    [field: SerializeField] public int XPosition {get; private set;} // Column
    [field: SerializeField] public int YPosition {get; private set;} // Row
    [field: SerializeField] public EAttribute CellAttribute {get; private set;}
    private Config CurrentConfig => CellConfigs.First(c => c.CellType == CellAttribute);
    public bool IsCharacterCell => Owner != null && Owner is Character;
    [ShowInInspector]
    public bool IsEnemyCell => Owner != null && Owner is Enemy;
    public bool IsConsumed { get;set; }

    public void SetupCell(EAttribute type, int xPos, int yPos)
    {
        IsConsumed = false;
        CellAttribute = type;
        XPosition = xPos;
        YPosition = yPos;
        CurrentConfig.NormalCell.SetActive(true);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }
    
    public void SetupSelect()
    {
        if (CurrentState == State.Hide) return;
        CurrentState = State.Selected;
        CurrentConfig.NormalCell.SetActive(false);
        CurrentConfig.SelectedCell.SetActive(true);
        CurrentConfig.UnSelectCell.SetActive(false);
        CurrentConfig.FocusCell.SetActive(false);
        LineSelect.SetActive(true);
        LineUnSelect.SetActive(false);
    }
    public void SetupUnSelect()
    {
        if (CurrentState == State.Hide) return;
        CurrentState = State.UnSelect;
        CurrentConfig.NormalCell.SetActive(false);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(true);
        CurrentConfig.FocusCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }
    public void SetupNormal()
    {
        if (CurrentState == State.Hide) return;
        CurrentState = State.Normal;
        CurrentConfig.NormalCell.SetActive(true);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        CurrentConfig.FocusCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }
    public void SetupHide()
    {
        CurrentState = State.Hide;
        CurrentConfig.NormalCell.SetActive(false);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        CurrentConfig.FocusCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(false);
    }
    public void SetupFocus()
    {
        if (CurrentState == State.Hide) return;
        CurrentState = State.Focus;
        CurrentConfig.NormalCell.SetActive(false);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        CurrentConfig.FocusCell.SetActive(true);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }
    public void SetupConsume()
    {
        if (CurrentState == State.Hide) return;
        SetupHide();
        IsConsumed = true;
        CurrentState = State.Normal;
        CellAttribute = EAttribute.Grey;
        CurrentConfig.NormalCell.SetActive(true);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }

    public void RandomNewColor()
    {
        IsConsumed = false;
        SetupHide();
        CellAttribute = GetRandomBasicType();
        CurrentState = State.Normal;
        CurrentConfig.NormalCell.SetActive(true);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }
    
    public List<Cell> GetCell(EAreaType areaType)
    {
        return CellManager.Instance.GetCell(areaType, this);
    }
    public bool IsCellInSmallArea(Cell cell)
    {
        return GetCell(EAreaType.SmallArea).Contains(cell);
    }
    public void RegisterOwner(Entity entity)
    {
        Owner = entity;
    }
    public void UnRegisterOwner()
    {
        Owner = null;
    }
}
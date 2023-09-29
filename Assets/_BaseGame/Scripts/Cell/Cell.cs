using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TW.Utility.CustomComponent;
using UnityEngine;

public partial class Cell : AwaitableCachedMonoBehaviour
{
    public enum State
    {
        Normal,
        Selected,
        UnSelect,
        Hide
    }
    [field: SerializeField] public Config[] CellConfigs {get; private set;}
    [field: SerializeField] public State CurrentState {get; private set;}
    [field: SerializeField] public GameObject LineSelect {get; private set;}
    [field: SerializeField] public GameObject LineUnSelect {get; private set;}
    [field: SerializeField] public Entity Owner {get; private set;}
    [field: SerializeField] private int XPosition {get; set;}
    [field: SerializeField] private int YPosition {get; set;}
    [field: SerializeField] public EAttribute CellType {get; private set;}
    private Config CurrentConfig => CellConfigs.First(c => c.CellType == CellType);
    public void SetupCell(EAttribute type)
    {
        CellType = type;
        CurrentConfig.NormalCell.SetActive(true);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(false);
    }
    public void SetupSelect()
    {
        if (CurrentState == State.Hide) return;
        CurrentState = State.Selected;
        CurrentConfig.NormalCell.SetActive(false);
        CurrentConfig.SelectedCell.SetActive(true);
        CurrentConfig.UnSelectCell.SetActive(false);
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
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(true);
    }
    public void SetupHide()
    {
        CurrentState = State.Hide;
        CurrentConfig.NormalCell.SetActive(false);
        CurrentConfig.SelectedCell.SetActive(false);
        CurrentConfig.UnSelectCell.SetActive(false);
        LineSelect.SetActive(false);
        LineUnSelect.SetActive(false);
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
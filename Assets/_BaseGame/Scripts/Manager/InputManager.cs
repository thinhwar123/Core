using System.Collections.Generic;
using TW.Utility.DesignPattern;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    [field: SerializeField] private bool IsTouchingUI { get; set; }
    [field: SerializeField] private LayerMask WhatIsCell { get; set; }
    [field: SerializeField] private EAttribute SelectEAttribute { get; set; }
    private List<Cell> CellList { get; set; } = new List<Cell>();

    private void Awake()
    {
        SelectEAttribute = EAttribute.Clear;
    }

    private void Update()
    {
        HandleMouseInGameInput();
    }

    private void HandleMouseInGameInput()
    {
        if (IsPointerOverGameObject()) return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CellList.Clear();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            CameraManager.Instance.CalculateTargetPosition();
            Ray ray = CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 200, WhatIsCell))
            {
                Cell hitCell = CacheComponent.GetCell(hitInfo.collider);
                if (CellList.Count == 0 && hitCell.CurrentState == Cell.State.Normal)
                {
                    CameraManager.Instance.SetFocusZoom();
                    SelectEAttribute = hitCell.CellAttribute;
                    CellManager.Instance.DeActiveAllOtherCell(SelectEAttribute);
                    CellManager.Instance.FocusAllCell(SelectEAttribute);
                    CellList.Add(hitCell);
                    hitCell.SetupSelect();
                }

                if (!CellList.Contains(hitCell) && hitCell.CurrentState == Cell.State.Focus &&
                    hitCell.CellAttribute == SelectEAttribute)
                {
                    if (CellList[^1].IsCellInSmallArea(hitCell))
                    {
                        CellList.Add(hitCell);
                        hitCell.SetupSelect();
                    }
                }

                if (CellList.Contains(hitCell) && CellList[^1] != hitCell)
                {
                    while (hitCell != CellList[^1])
                    {
                        CellList[^1].SetupFocus();
                        CellList.RemoveAt(CellList.Count - 1);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SelectEAttribute = EAttribute.Clear;
            if (CellList.Count > 0)
            {
                CameraManager.Instance.SetDefaultZoom();
            }

            CellManager.Instance.NormalAllCell();
            if (CellList.Count > 1)
            {
                TeamManager.Instance.TeamMoveFollowPath(CellList);
            }
        }
    }

    private bool IsPointerOverGameObject()
    {
        //check mouse
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                IsTouchingUI = true;
                return true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && IsTouchingUI)
        {
            IsTouchingUI = false;
            return true;
        }

        return IsTouchingUI;
    }
}
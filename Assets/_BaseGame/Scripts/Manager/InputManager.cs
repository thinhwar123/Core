using System.Collections.Generic;
using System.Linq;
using TW.Utility.DesignPattern;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    [field: SerializeField] private bool IsTouchingUI { get; set; }
    [field: SerializeField] private LayerMask WhatIsCell { get; set; }
    [field: SerializeField] private EAttribute SelectEAttribute { get; set; }
    [field: SerializeField] private GameObject LinkPrefab { get; set; }
    private List<Cell> CellList { get; set; } = new List<Cell>();
    private List<GameObject> LinkList { get; set; } = new List<GameObject>();

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
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.PlayerTurn) return;
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
                if (CellList.Count == 0 && hitCell.CurrentState == Cell.State.Normal && !hitCell.IsEnemyCell && !hitCell.IsCharacterCell)
                {
                    CameraManager.Instance.SetFocusZoom();
                    SelectEAttribute = hitCell.CellAttribute;
                    CellManager.Instance.DeActiveAllOtherCell(SelectEAttribute);
                    CellManager.Instance.FocusAllCell(SelectEAttribute);
                    CellList.Add(hitCell);
                    hitCell.SetupSelect();
                    UpdateLink();
                }

                if (CellList.Count > 0 && !CellList.Contains(hitCell) && hitCell.CurrentState == Cell.State.Focus &&
                    hitCell.CellAttribute == SelectEAttribute && !hitCell.IsEnemyCell && !hitCell.IsCharacterCell)
                {
                    if (CellList[^1].IsCellInSmallArea(hitCell))
                    {
                        CellList.Add(hitCell);
                        hitCell.SetupSelect();
                        UpdateLink();
                    }
                }

                if (CellList.Contains(hitCell) && CellList[^1] != hitCell)
                {
                    while (hitCell != CellList[^1])
                    {
                        CellList[^1].SetupFocus();
                        CellList.RemoveAt(CellList.Count - 1);
                        UpdateLink();
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
                TeamManager.Instance.TeamMoveFollowPath(CellList.ToList());
            }
            CellList.Clear();
            ClearLink();
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

    private void UpdateLink()
    {
        ClearLink();
        if(CellList.Count < 2) return;
        for (int i = 0; i < CellList.Count - 1; i++)
        {
            Vector3 direction = CellList[i + 1].transform.position - CellList[i].transform.position;
            Vector3 position = CellList[i].transform.position + direction / 2f;
            float distance = direction.magnitude;
            
            GameObject link = Instantiate(LinkPrefab, position, Quaternion.identity);
            link.transform.localScale = new Vector3(1, 1, distance);
            link.transform.rotation = Quaternion.LookRotation(direction);
            LinkList.Add(link);
        }
    }
    private void ClearLink()
    {
        LinkList.ForEach(Destroy);
        LinkList.Clear();
    }
}
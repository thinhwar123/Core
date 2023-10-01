using System.Collections.Generic;
using System.Linq;
using BaseGame;
using TW.Utility.DesignPattern;
using UnityEngine;
using Sirenix.Utilities;
using TW.UI.CustomComponent;
using TW.Utility.Extension;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PlayerTurn,
        CalculateTurn,
        EnemyTurn,
        SelectLevel,
    }
    
    [field: SerializeField] public UIHealthBar UIHealthBarPrefab {get; private set;}
    [field: SerializeField] public UIDamagePopup UIDamagePopupPrefab {get; private set;}
    [field: SerializeField] public Transform UIInGameContainer {get; private set;}
    [field: SerializeField] public GameState CurrentGameState {get; private set;}
    private MapDataModel dataModel;
    
    private void Start()
    {
        AUIManager.Instance.OpenUI<PopupMapUI>();
        SetGameState(GameState.SelectLevel);
    }
    public void CreateMap(int treeFloor, int mapId)
    {
        Debug.Log("Start Level");
        ClearAllMap();
        
        // MapDBModel mapDBModel = MapManager.Instance.ListMapDBModel.listMapDBModel.First(x => x.treeFloor == treeFloor && x.id == mapId);
        MapDataModel mapDataModel = MapManager.Instance.mapAssetData.listMapDataModel.First(x => x.mapID == mapId && x.mapTree == treeFloor);
        dataModel = mapDataModel;
        CellManager.Instance.CreateMap();

        List<BaseGame.EnemyConfig> listEnemy = mapDataModel.listMapDetailModel[0].listEnemy;
        int[] cellIndex = mapDataModel.listMapDetailModel[0].posEnemy.AreaPosition;
        int[] enemyCells = cellIndex.Where(x => x == 2).ToArray();
        List<int> enemyIndexList = new List<int>();
        for (int i = 0; i < cellIndex.Length; i++)
        {
            if (cellIndex[i] == 2)
            {
                enemyIndexList.Add(i);
            }
        }
        List<Cell> enemyCell = enemyIndexList
            .Select(x => CellManager.Instance.GetCell(x))
            .Shuffle()
            .ToList();
        EnemyManager.Instance.InitEnemy(listEnemy, enemyCell);
        TeamManager.Instance.InitTeam();
        SetGameState(GameState.PlayerTurn);
    }
    
    public UIHealthBar CreateUIHealthBar()
    {
        return Instantiate(UIHealthBarPrefab, UIInGameContainer);
    }
    public UIDamagePopup CreateUIDamagePopup()
    {
        return Instantiate(UIDamagePopupPrefab, UIInGameContainer);
    }
    public void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
    }

    public void OnWinGame()
    {
        Debug.Log("Win Game");
        ClearAllMap();
        AUIManager.Instance.OpenUI<UIResult>().SetupOnOpen(true);
        AUIManager.Instance.CloseUI<UIInGame>();
    }
    public void OnLoseGame()
    {
        Debug.Log("Lose Game");
        ClearAllMap();
        AUIManager.Instance.OpenUI<UIResult>().SetupOnOpen(false);
        AUIManager.Instance.CloseUI<UIInGame>();
    }

    public void ClearAllMap()
    {
        SetGameState(GameState.SelectLevel);
        CellManager.Instance.ClearMap();
        EnemyManager.Instance.ClearEnemy();
        TeamManager.Instance.ClearTeam();
    }
    public void OnReplay(){
        if (dataModel != null){
            CreateMap(dataModel.mapTree, dataModel.mapID);
        }else{
            CreateMap(0, 0);
        }
    }
}

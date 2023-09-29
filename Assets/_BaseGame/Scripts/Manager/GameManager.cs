using System;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.DesignPattern;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PlayerTurn,
        CalculateTurn,
        EnemyTurn,
    }
    
    [field: SerializeField] public UIHealthBar UIHealthBarPrefab {get; private set;}
    [field: SerializeField] public Transform UIHealthBarContainer {get; private set;}
    [field: SerializeField] public GameState CurrentGameState {get; private set;}
    private void Start()
    {
        CellManager.Instance.CreateMap();
        TeamManager.Instance.InitTeam();
        EnemyManager.Instance.InitDemoEnemy();
    }
    
    public UIHealthBar CreateUIHealthBar()
    {
        return Instantiate(UIHealthBarPrefab, UIHealthBarContainer);
    }
    public void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
    }
}

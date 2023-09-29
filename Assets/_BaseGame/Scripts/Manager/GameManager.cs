using System;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.DesignPattern;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        CellManager.Instance.CreateMap();
        TeamManager.Instance.InitTeam();
    }
}

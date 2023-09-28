
using System.Collections.Generic;

public class ListMapDBModel
{
    public List<MapDBModel> listMapDBModel;
}


public class MapDBModel
{
    public int treeFloor { get; set; }// id tree Floor
    public int id { get; set; }// id map
    public bool isUnlock { get; set; }// check is unlock
    public bool isFinish { get; set; }// check is finish
    public int numbSlotUnlock {get;set;}
}


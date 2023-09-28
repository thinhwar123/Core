using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame
{
    [System.Serializable]
    public class MapDataModel
    {
        public int mapTree;// id tree
        public int mapID;// id map
        public Sprite sprMap;// id map spr
        public List<MapDetailDataModel> listMapDetailModel;// list map
        public List<LevelSlotRequestModel> mapSlotRequests;// list map request
        public string description;// map des show on ui
        public string name;// name map
        public void CopyFromOther(MapDataModel other)
        {
            mapTree = other.mapTree;
            mapID = other.mapID;
            sprMap = other.sprMap;
            description = other.description;
            name = other.name;

            if (listMapDetailModel == null)
                listMapDetailModel = new List<MapDetailDataModel>();
            listMapDetailModel.Clear();
            for (int i = 0; i < other.listMapDetailModel.Count; i++)
            {
                MapDetailDataModel mapDetailModel = new MapDetailDataModel();
                mapDetailModel.CopyFromOther(other.listMapDetailModel[i]);
                listMapDetailModel.Add(mapDetailModel);
            }
            // slot request
            if (mapSlotRequests == null)
                mapSlotRequests = new List<LevelSlotRequestModel>();
            mapSlotRequests.Clear();
            for (int i = 0; i < other.mapSlotRequests.Count; i++)
            {
                LevelSlotRequestModel mapDetailData = new LevelSlotRequestModel();
                mapDetailData.CopyFromOther(other.mapSlotRequests[i]);
                mapSlotRequests.Add(mapDetailData);
            }
        }
    }
    [System.Serializable]
    public class LevelSlotRequestModel
    {
        public int levelTree;
        public int levelID;
        public void CopyFromOther(LevelSlotRequestModel other){
            levelTree = other.levelTree;
            levelID = other.levelID;
        }
    }
    [System.Serializable]
    public class MapDetailDataModel
    {
        public float startLevelValue; // gia tri skill level 1
        public float levelUpValue; // next level dc cong them


        public void CopyFromOther(MapDetailDataModel other)
        {
            startLevelValue = other.startLevelValue;
            levelUpValue = other.levelUpValue;
        }
    }

}

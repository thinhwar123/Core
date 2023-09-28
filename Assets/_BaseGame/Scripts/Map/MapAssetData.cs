using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame
{
    [CreateAssetMenu(fileName = "MapAssetData", menuName = "Data/Map Asset Data", order = 1)]
    public class MapAssetData : ScriptableObject
    {
        public List<MapDataModel> listMapDataModel = new List<MapDataModel>();

        public MapDataModel GetMapWithData(int treeID, int id)
        {
            return listMapDataModel.Find(e => e.mapTree == treeID && e.mapID == id);
        }
    }

}
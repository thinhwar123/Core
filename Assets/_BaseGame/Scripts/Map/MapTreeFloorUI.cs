
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame
{
    public class MapTreeFloorUI : MonoBehaviour
    {
        [SerializeField] public List<MapCellUI> listMapCell;
        [HideInInspector] public int floor;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="floor"></param>
        /// <param name="listMapData"></param>

        public void SetFloorData(int _floor, List<MapDataModel> listMapData)
        {
            floor = _floor;
            for (int i = 0; i < listMapData.Count; i++)
            {
                if (i >= listMapCell.Count)
                {
                    var researchCell = Instantiate(listMapCell[0], listMapCell[0].transform.parent).GetComponent<MapCellUI>();
                    listMapCell.Add(researchCell);
                }
                listMapCell[i].gameObject.SetActive(true);
                listMapCell[i].SetMapData(listMapData[i]);
                // if (floor == 0 && i == 0&& MapDetailUI.GetInstance().CurCellSelect == null)
                // if (floor == 0 && i == 0)
                //     listMapCell[i].SelectMap();
            }


        }
    }

}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

namespace BaseGame
{
    public class PopupMapUI : Singleton<PopupMapUI>
    {
        [Header("Data")]
        [SerializeField] MapAssetData masterAssetData;
        [Header("UI")]
        [SerializeField] List<MapTreeFloorUI> listMapTreeCell;
        [SerializeField] MapDetailUI mapDetailUI;
        public bool isLoadingUI { set; get; }

        bool isLoadUI = false;
        public bool isLoadData { set; get; }


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(IELoadUI());
        }


        IEnumerator IELoadUI()
        {
            isLoadUI = true;
            isLoadingUI = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            while (MapManager.Instance.ListMapDBModel == null)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            for (int i = 0; i <= masterAssetData.listMapDataModel[masterAssetData.listMapDataModel.Count - 1].mapTree; i++)
            {
                var listMapFloor = masterAssetData.listMapDataModel.FindAll(e => e.mapTree == i);
                if (i >= listMapTreeCell.Count)
                {
                    var researchFloorCell = Instantiate(listMapTreeCell[0], listMapTreeCell[0].transform.parent).GetComponent<MapTreeFloorUI>();
                    listMapTreeCell.Add(researchFloorCell);
                }
                listMapTreeCell[i].gameObject.SetActive(true);
                listMapTreeCell[i].SetFloorData(i, listMapFloor);
            }
            yield return new WaitForEndOfFrame();
            isLoadingUI = false;
        }

        public void ReloadUI()
        {
            StartCoroutine(IELoadUI());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeFloor"></param>
        /// <param name="mapID"></param>
        /// <returns></returns>
        public MapCellUI GetMapCell(int treeFloor, int mapID)
        {
            return listMapTreeCell.Find(e => e.floor == treeFloor)?.listMapCell.Find(e => e.MapDataModel.mapID == mapID);
        }

        public void SelectMapCell(MapCellUI cellUI)
        {
            mapDetailUI.SetMapData(cellUI);
        }

        public MapDetailUI GetMapDetailUIForTut()
        {
            return mapDetailUI;
        }
    }
}

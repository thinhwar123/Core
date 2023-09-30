using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;
using TW.UI.CustomComponent;
using TW.Utility.CustomComponent;

namespace BaseGame
{
    public class PopupMapUI : AUIPanel
    {
        [field: SerializeField] private AUIButton GachaButton {get; set;}
        [Header("Data")]
        [SerializeField] MapAssetData masterAssetData;
        [Header("UI")]
        [SerializeField] List<MapTreeFloorUI> listMapTreeCell;
        //[SerializeField] MapDetailUI mapDetailUI;
        public bool isLoadingUI { set; get; }

        bool isLoadUI = false;
        public bool isLoadData { set; get; }
        protected override void Awake()
        {
            base.Awake();
            GachaButton.OnClickButton.AddListener(OnClickGachaButton);
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(IELoadUI());
        }

        private void OnClickGachaButton()
        {
            AUIManager.Instance.OpenUI<UIGachaWibu>().SetupOnOpen();
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
            GameManager.Instance.CreateMap(cellUI.MapDataModel.mapTree, cellUI.MapDataModel.mapID);
            AUIManager.Instance.OpenUI<UIInGame>().SetupOnOpen(TeamManager.Instance.CharacterConfigs);
            AUIManager.Instance.CloseUI<PopupMapUI>();
        }

        // public MapDetailUI GetMapDetailUIForTut()
        // {
        //     return mapDetailUI;
        // }
    }
}

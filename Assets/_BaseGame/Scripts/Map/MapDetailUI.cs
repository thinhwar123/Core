using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace BaseGame
{
    public class MapDetailUI : MonoBehaviour
    {
        private static MapDetailUI _instance;
        [SerializeField] TMP_Text lblName;
        [SerializeField] TMP_Text lblPoint;
        [SerializeField] Button btnUpgrade;
        [SerializeField] RectTransform timeMapLayout;
        [SerializeField] TMP_Text lblTimeMap;
        [SerializeField] MapCellUI mapCellUI;
        MapDBModel mapDBModel;
        MapDataModel mapDataModel;
        MapCellUI selectCellUI;
        public MapCellUI CurCellSelect => selectCellUI;
        [SerializeField] TMP_Text lblCurData;
        [SerializeField] TMP_Text lblCurData2;
        [SerializeField] TMP_Text lblNextdata;
        [SerializeField] GameObject currentObj;
        [SerializeField] GameObject upgradeObj;

        int gemUpgrade;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MapDetailUI GetInstance()
        {
            return _instance;
        }

        private void Start()
        {
            _instance = this;
        }

        public void SetMapData(MapCellUI _mapCellUI)
        {
            if (selectCellUI != null)
                selectCellUI.ShowSelectObj(false);
            selectCellUI = _mapCellUI;
            selectCellUI.ShowSelectObj(true);
            if (mapDBModel != null)
            {
                //MapManager.Instance.SetCounterLabel(mapDBModel.treeFloor * 1000 + mapDBModel.id, null);
            }

            mapDBModel = selectCellUI.MapDBModel;
            mapDataModel = selectCellUI.MapDataModel;
            lblName.text = mapDataModel.name;
            // var mapValue = mapDataModel.GetTotalMapDataValue(1);
            // lblCurData.text = string.Format(mapDataModel.description, string.Format("<color=green>{0}</color>", mapValue));

            currentObj.SetActive(true);
            upgradeObj.SetActive(false);

            // var mapDetail = mapDataModel.listMapDetail[mapDBModel.mapCount % mapDataModel.listMapDetail.Count];
            // lblPoint.text = mapDetail.mapPoint.ToString();
            mapCellUI.SetMapData(mapDataModel, false);

            btnUpgrade.interactable = !mapDBModel.isUnlock && !mapDBModel.isFinish;
            btnUpgrade.gameObject.SetActive(!mapDBModel.isFinish);
            // if (mapDBModel.isFinish)
            // {
            //     lblTimeMap.text = "Max";
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(timeMapLayout);
            // }
            // else
            // {
            //     if (!mapDBModel.isMap)
            //     {
            //         int mapTime = (int)(mapDetail.mapTime / (1 + MapManager.Instance.MapBuffModel.speed_map / 100.0f));
            //         lblTimeMap.SetText(
            //             mapTime < 3600
            //                 ? $"{(mapTime / 60).ToString("00")}:{(mapTime % 60).ToString("00")}"
            //                 : $"{(mapTime / 3600).ToString("00")}:{((mapTime % 3600) / 60).ToString("00")}:{(mapTime % 60).ToString("00")}");
            //         if (mapDBModel.isUnlock)
            //             btnUpgrade.interactable = CraftManager.Instance.GamePointDBModel.numbMapPoint >= mapDetail.mapPoint;
            //         else
            //             btnUpgrade.interactable = false;
            //         LayoutRebuilder.ForceRebuildLayoutImmediate(timeMapLayout);
            //     }
            //     else
            //     {
            //     }
            // }
        }


        // reload data
        public void ReloadData(MapDBModel mapDB = null)
        {
            if (selectCellUI != null)
                SetMapData(selectCellUI);
        }

        #region Action

        public void MapAction()
        {
            btnUpgrade.interactable = false;
            mapDBModel.isUnlock = true;
            // mapDBModel.lastimeMap = DateTime.Now.ToString(Helper.Date_Time_Format);
            // PopupMapControl.Instance.UpdateUIData();
            // MapManager.Instance.CheckMap(mapDBModel, mapDataModel);
            // MapManager.Instance.Save();
            btnUpgrade.gameObject.SetActive(false);
            selectCellUI.researingObj.SetActive(true);
            SetMapData(selectCellUI);
        }

        #endregion
    }
}
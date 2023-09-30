using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using HLib.Jobs;
using HLib.IO;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace BaseGame
{
    public class MapManager : Singleton<MapManager>
    {
        [field: Header("Data")]
        [field: SerializeField] public MapAssetData mapAssetData { get; private set; }
        [SerializeField] GameObject notifiMap;

        [HideInInspector] public bool isLoadData = false;
        // data
        ListMapDBModel listMapDBModel;
        public ListMapDBModel ListMapDBModel => listMapDBModel;

        private void Start()
        {
            StartCoroutine(IEGetData());
        }

        #region Get Data

        IEnumerator IEGetData()
        {
            yield return new WaitForEndOfFrame();
            HJobs.Create();
            listMapDBModel = HFile.LoadDataLocal<ListMapDBModel>();
            if (listMapDBModel == null)
                listMapDBModel = new ListMapDBModel();
            if (listMapDBModel.listMapDBModel == null)
                listMapDBModel.listMapDBModel = new List<MapDBModel>();
            if (listMapDBModel.listMapDBModel.Count == 0)
            {
                Reset();
            }

        }

        [ContextMenu("Reset")]
        /// <summary>
        /// reset and create default
        /// </summary>
        public void Reset()
        {
            Default();
            Save();
        }

        private void Default()
        {
            if (listMapDBModel == null)
                listMapDBModel = new ListMapDBModel();
            if (listMapDBModel.listMapDBModel == null)
                listMapDBModel.listMapDBModel = new List<MapDBModel>();
            listMapDBModel.listMapDBModel.Clear();
            for (int i = 0; i < mapAssetData.listMapDataModel.Count; i++)
            {
                var dataModel = mapAssetData.listMapDataModel[i];
                MapDBModel mapDBModel = new MapDBModel();
                mapDBModel.id = dataModel.mapID;
                mapDBModel.treeFloor = dataModel.mapTree;
                mapDBModel.isUnlock = false;
                listMapDBModel.listMapDBModel.Add(mapDBModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            HFile.SaveLocal<ListMapDBModel>(listMapDBModel);
        }

        #endregion


        public void SelectMapTab()
        {
            // var popup = PopupManager.CreateNewInstance<PopupSystemMap>();
            // popup.GetComponent<PopupController>().ShowPopup();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TW.UI.CustomComponent;
using TW.Utility.CustomComponent;

namespace BaseGame
{
    public class MapCellUI : MonoBehaviour
    {
        [SerializeField] Image imgActive;
        [SerializeField] Image imgMap;
        [SerializeField] public GameObject mapLine;
        [SerializeField] public GameObject researingObj;
        [SerializeField] GameObject selectOBj;
        // [SerializeField] GameObject maxLevel;
        // [SerializeField] MapDetailUI mapDetail;
        [SerializeField] Sprite sprBgFinish;
        [SerializeField] Sprite sprBgMap;
        MapDBModel mapDBModel;
        public MapDBModel MapDBModel => mapDBModel;

        MapDataModel mapDataModel;
        public MapDataModel MapDataModel => mapDataModel;

        //private void OnEnable()
        //{
        //    if(mapDBModel!=null)
        //    {

        //    }
        //}

        #region set Data
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_mapDBModel"></param>
        /// <param name="_mapDataModel"></param>
        public void SetMapData(MapDataModel _mapDataModel, bool showLine = true)
        {
            // if (MapDetailUI.GetInstance().CurCellSelect==null)
            //     selectOBj.SetActive(false);
            // else
            //     selectOBj.SetActive(this == MapDetailUI.GetInstance().CurCellSelect);

            mapDBModel = MapManager.Instance.ListMapDBModel.listMapDBModel.Find(e => e.treeFloor == _mapDataModel.mapTree && e.id == _mapDataModel.mapID);
            mapDataModel = _mapDataModel;
            imgMap.sprite = mapDataModel.sprMap;
            //maxLevel.SetActive(mapDBModel.isFinish);
            if (showLine)
                StartCoroutine(IECheckLine());
            var listSlotRequest = mapDataModel.mapSlotRequests;
            if (listSlotRequest.Count > 0)
            {
                imgActive.fillAmount = 1;
            }
            else
            {
                imgActive.fillAmount = 1;
            }
            if (mapDBModel.isFinish)
                imgActive.sprite = sprBgFinish;
            else
                imgActive.sprite = sprBgMap;

        }

        IEnumerator IECheckLine()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            while (AUIManager.Instance.GetUI<PopupMapUI>().isLoadingUI)
            {
                yield return new WaitForEndOfFrame();
            }
            // draw list line
            int numbUnlock = 0;
            var listSlotRequest = mapDataModel.mapSlotRequests;
            for (int i = 0; i < listSlotRequest.Count; i++)
            {
                var slotRequest = listSlotRequest[i];
                var slotRequestCell = AUIManager.Instance.GetUI<PopupMapUI>().GetMapCell(slotRequest.levelTree, slotRequest.levelID);
                var line = Instantiate(slotRequestCell.mapLine, slotRequestCell.mapLine.transform.parent);
                
                if( Mathf.Abs(mapDataModel.mapTree - slotRequest.levelTree) >= 2)
                {
                    var rectTransForm = line.GetComponent<RectTransform>();
                    rectTransForm.sizeDelta =new Vector2(rectTransForm.sizeDelta.x, rectTransForm.sizeDelta.y * Mathf.Abs(mapDataModel.mapTree - slotRequest.levelTree));
                }
                // check line unlock or not
                bool isUnlock = MapManager.Instance.ListMapDBModel.listMapDBModel.Find(e => e.treeFloor == slotRequest.levelTree && e.id == slotRequest.levelID).isFinish;
                line.GetComponent<MapLineUI>().SetLineState(isUnlock);
                if (isUnlock)
                    numbUnlock++;
                // rotate line with request cell
                Vector2 despoint = slotRequestCell.transform.position;
                Vector2 startPoint = transform.position;
                line.transform.localPosition = -(despoint - startPoint)/2;
                var direction = startPoint - despoint;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                RectTransform lineRect = line.GetComponent<RectTransform>();
                lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, direction.magnitude);
                line.gameObject.SetActive(true);
            }
            mapDBModel.numbSlotUnlock = numbUnlock;
            mapDBModel.isUnlock = numbUnlock == listSlotRequest.Count;
            // if (listSlotRequest.Count > 0)
            // {
            //     imgActive.fillAmount = numbUnlock * 1.0f / listSlotRequest.Count;
            // }
            //else
            {
                imgActive.fillAmount = 1;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void SelectMap()
        {
            AUIManager.Instance.GetUI<PopupMapUI>().SelectMapCell(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        public void ShowSelectObj(bool show)
        {
            selectOBj.SetActive(show);
        }

        public void ReloadUI(bool showLine = false)
        {
            imgMap.sprite = mapDataModel.sprMap;
            var listSlotRequest = mapDataModel.mapSlotRequests;
            if (listSlotRequest.Count > 0)
            {
                imgActive.fillAmount = mapDBModel.numbSlotUnlock * 1.0f / listSlotRequest.Count;
            }
            else
            {
                imgActive.fillAmount = 1;
            }
            if (mapDBModel.isFinish)
                imgActive.sprite = sprBgFinish;
            else
                imgActive.sprite = sprBgMap;
            CheckCurrentLine();
            if (showLine)
                CheckUnlock();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckUnlock()
        {
            int numbUnlock = 0;
            var listSlotRequest = mapDataModel.mapSlotRequests;
            for (int i = 0; i < listSlotRequest.Count; i++)
            {
                var slotRequest = listSlotRequest[i];
                var slotRequestCell = AUIManager.Instance.GetUI<PopupMapUI>().GetMapCell(slotRequest.levelTree, slotRequest.levelID);
                // check line unlock or not
                bool isUnlock = MapManager.Instance.ListMapDBModel.listMapDBModel.Find(e => e.treeFloor == slotRequest.levelTree && e.id == slotRequest.levelID).isFinish;
                if (isUnlock)
                    numbUnlock++;
            }
            mapDBModel.numbSlotUnlock = numbUnlock;
            mapDBModel.isUnlock = numbUnlock == listSlotRequest.Count;
            if (listSlotRequest.Count > 0)
            {
                imgActive.fillAmount = numbUnlock * 1.0f / listSlotRequest.Count;
            }
            else
            {
                imgActive.fillAmount = 1;
            }
        }
        public void CheckCurrentLine()
        {
            for (int i = 0; i < mapLine.transform.parent.childCount; i++)
            {
                var line = mapLine.transform.parent.GetChild(i);
                if (line.gameObject.activeSelf)
                {
                    line.GetComponent<MapLineUI>().SetLineState(mapDBModel.isFinish);
                }
            }
        }
    }
}

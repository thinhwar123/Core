
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABIPlugins;

namespace ABIPlugins
{
    public class InGamePopupController : MonoBehaviour
    {
        public Canvas canvas;
        public bool usingDefaultTransparent = true;
        public BasePopup[] prefabs;
        public GameObject transparent;
        private Transform mTransparentTrans;
        public Stack<BasePopup> popupStacks = new Stack<BasePopup>();
        public Transform parent;
        private int defaultSortingOrder;

        private static InGamePopupController mInstance;
        public static InGamePopupController Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<InGamePopupController>();
                    if (mInstance == null) LoadResource<InGamePopupController>("PopupManager");
                }

                return mInstance;
            }
        }

        void Awake()
        {
            mInstance = this;
            mTransparentTrans = transparent.transform;
            defaultSortingOrder = canvas.sortingOrder;
        }

        public static T CreateNewInstance<T>()
        {
            T result = Instance.CheckInstancePopupPrebab<T>();
            return result;
        }

        public T CheckInstancePopupPrebab<T>()
        {
            System.Type type = typeof(T);
            GameObject go = null;
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (IsOfType<T>(prefabs[i]))
                {
                    go = (GameObject)Instantiate(prefabs[i].gameObject, parent);
                    break;
                }
            }
            T result = go.GetComponent<T>();
            return result;
        }

        private bool IsOfType<T>(object value)
        {
            return value is T;
        }

        public void ChangeTransparentOrder(Transform topPopupTransform, bool active)
        {
            if (active)
            {
                mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 1);
                transparent.SetActive(true && usingDefaultTransparent);
            }
            else
            {
                if (parent.childCount > 2)
                {
                    mTransparentTrans.SetSiblingIndex(topPopupTransform.GetSiblingIndex() - 2);
                }
                else
                {
                    transparent.SetActive(false);
                }
            }
        }


        public InGamePopupController Preload()
        {
            return mInstance;
        }

        public bool SequenceHidePopup()
        {
            if (popupStacks.Count > 0)
            {
                BasePopup bp = popupStacks.Peek();
                if (bp != null)
                {
                    bp.Hide();
                }
                else
                {
                    popupStacks.Pop();
                }
            }
            else
                transparent.SetActive(false);

            return (popupStacks.Count > 0);
        }

        public static T LoadResource<T>(string name)
        {
            GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(name));
            go.name = string.Format("[{0}]", name);
            DontDestroyOnLoad(go);
            return go.GetComponent<T>();
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }

        public void ResetOrder()
        {
            canvas.sortingOrder = defaultSortingOrder;
        }

        public void ReloadCamera()
        {
            //canvas.worldCamera = Camera.main;
        }
    }
}
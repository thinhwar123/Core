using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ABIPlugins
{
    [RequireComponent(typeof(Animator))]
    public class BasePopup : MonoBehaviour
    {
        /// <summary>
        /// Optional Animator show popup controller
        /// </summary>
        [HideInInspector]
        public Animator animator;

        /// <summary>
        /// Optional animation show
        /// </summary>
        public AnimationClip showAnimationClip;

        /// <summary>
        /// Optional animation hide
        /// </summary>
        public AnimationClip hideAnimationClip;

        protected bool isShowed;
        private int mSortOrder;
        private Transform mTransform;
        private bool overlay;
        private Stack<BasePopup> refStacks;
        private Action hideCompletedCallback;
        private Action showCompletedCallback;


        public virtual void Awake()
        {
            isShowed = false;
            animator = GetComponent<Animator>();
            mTransform = transform;
            mSortOrder = mTransform.GetSiblingIndex();
            refStacks = PopupManager.Instance.popupStacks;
          
        }

        protected void Show(Action showCompletedCallback = null, bool overlay = true, Action hideCompletedCallback = null)
        {
            //Trường hợp chỉ tạo popup duy nhất.
            if (isShowed) 
            {
                Reshow();
                int topSortOrder = refStacks.Peek().SortOrder();
                //Nếu đã bị các popup khác đè lên
                if (refStacks.Count > 1 && SortOrder() != topSortOrder)
                {
                    // Đẩy popup này lên trên cùng và sắp xếp lại sortOrder cho toàn stack
                    MoveElementToTopStack(ref refStacks, SortOrder()); 
                }
                return;
            }
            else
            {
                this.showCompletedCallback = showCompletedCallback;
                this.hideCompletedCallback = hideCompletedCallback;
            }

            float waitLastPopupHide = 0;
            this.overlay = overlay;
            isShowed = true;

            if (!overlay && refStacks.Count > 0)
                ForceHideAllCurrent(ref waitLastPopupHide);

            if (!refStacks.Contains(this))
                refStacks.Push(this);

            if (refStacks.Count > 0)
                ChangeSortOrder(refStacks.Peek().SortOrder() + 1);

            if (waitLastPopupHide != 0)
                Invoke("AnimateShow", waitLastPopupHide);
            else
                AnimateShow();
        }

        public void Reshow()
        {
            if (animator != null && showAnimationClip != null)
            {
                animator.Play(showAnimationClip.name, -1, 0.0f);
                float showAnimationDuration = GetAnimationClipDuration(showAnimationClip);
                Invoke("OnShowFinish", showAnimationDuration);
            }

            PopupManager.Instance.ChangeTransparentOrder(mTransform, true);
        }

        public virtual void Hide()
        {
            if (!isShowed)
                return; 
            isShowed = false;

            AnimateHide();
        }

        public void Hide(Action hideCompletedCallback)
        {
            this.hideCompletedCallback = hideCompletedCallback;
            if (!isShowed)
                return;

            isShowed = false;

            AnimateHide();
        }

        public void OnCloseClick()
        {
            Hide();
        }

        private void AnimateShow()
        {
            if (animator != null && showAnimationClip != null)
            {
                float showAnimationDuration = GetAnimationClipDuration(showAnimationClip);
                Invoke("OnShowFinish", showAnimationDuration);
                animator.Play(showAnimationClip.name);
            } else {
                OnShowFinish();
            }

            PopupManager.Instance.ChangeTransparentOrder(mTransform, true);
        }

        void OnShowFinish()
        {
            if (showCompletedCallback != null)
            {
                showCompletedCallback();
            }
        }

        private void AnimateHide()
        {
            PopupManager.Instance.ChangeTransparentOrder(mTransform, false);
            if (animator != null && hideAnimationClip != null)
            {
                animator.Play(hideAnimationClip.name);
                float hideAnimationDuration = GetAnimationClipDuration(hideAnimationClip);
                Invoke("Destroy", hideAnimationDuration);
            }
            else
            {
                Destroy();
            }
        }

        void Destroy()
        {
            if (refStacks.Contains(this))
                refStacks.Pop();

            if (gameObject.activeSelf)
                DestroyImmediate(gameObject);

            if (hideCompletedCallback != null) hideCompletedCallback();
            PopupManager.Instance.ResetOrder();
        }

        public bool IsShowed
        {
            get { return isShowed; }
        }

        public int SortOrder()
        {
            return mSortOrder;
        }

        public void ChangeSortOrder(int newSortOrder = -1)
        {
            if (newSortOrder != -1)
            {
                mTransform.SetSiblingIndex(newSortOrder);
                mSortOrder = newSortOrder;
            }
        }

        private void ForceHideAllCurrent(ref float waitTime)
        {
            while (refStacks.Count > 0)
            {
                BasePopup bp = refStacks.Pop();
                waitTime += bp.GetAnimationClipDuration(hideAnimationClip);
                bp.Hide();
            }
        }

        private float GetAnimationClipDuration(AnimationClip clip)
        {
            if (animator != null && clip != null)
            {
                RuntimeAnimatorController rac = animator.runtimeAnimatorController;
                for (int i = 0; i < rac.animationClips.Length; i++)
                {
                    if (rac.animationClips[i].Equals(clip))
                        return rac.animationClips[i].length;
                }
            }

            return 0;
        }

        private void MoveElementToTopStack(ref Stack<BasePopup> stack, int order)
        {
            Stack<BasePopup> tempStack = new Stack<BasePopup>();
            BasePopup foundPopup = null;
            int minSortOrder = 0;
            while (refStacks.Count > 0)
            {
                BasePopup bp = refStacks.Pop();
                if (bp.SortOrder() != order)
                {
                    tempStack.Push(bp);
                    minSortOrder = bp.SortOrder();
                }
                else
                {
                    foundPopup = bp;
                }
            }

            while (tempStack.Count > 0)
            {
                BasePopup bp = tempStack.Pop();
                bp.ChangeSortOrder(minSortOrder++);
                stack.Push(bp);
            }

            if (foundPopup != null)
            {
                foundPopup.ChangeSortOrder(minSortOrder);
                stack.Push(foundPopup);
            }
        }
    }
}
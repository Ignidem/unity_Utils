using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Axis = UnityEngine.RectTransform.Axis;

namespace UnityUtils.DynamicScrollers.Foldables
{
    public class FoldableCell : MonoBehaviour, IFoldableCell
    {
        public virtual Type CellType => GetType();
        public int CellIndex { get; set; }
        public int DataIndex { get; set; }
        public RectTransform Transform => transform as RectTransform;
        
        public bool IsFolded { get; private set; }
        public bool IsParentFolded { get; private set; }

        [field: SerializeField]
        public List<FoldableCell> containedCells { get; private set; }
        
        public virtual Vector2 GetSize(Rect container, Axis axis) => Vector2.zero;
        public virtual Task<bool> SetData(IScrollerCellData data)
        {
            if (data is IFoldableData _data)
                SetFolded(_data.IsFolded);
            
            return Task.FromResult(true);
        }

        public virtual void Refresh() => SetFolded(IsFolded);
        public virtual void Clear() { }
        public void ToggleFolded() => SetFolded(!IsFolded);
        public virtual void SetFolded(bool isFolded)
        {
            IsFolded = isFolded;
            foreach (FoldableCell cell in containedCells)
            {
                cell.OnParentFoldChange(isFolded);
            }
        }
        public virtual void OnParentFoldChange(bool parentFolded)
        {
            IsParentFolded = parentFolded;
            OnFoldedInHierarchy(parentFolded);
        }

        protected virtual void OnFoldedInHierarchy(bool isFolded)
        {            
            gameObject.SetActive(!isFolded && !IsParentFolded);
            
            foreach (FoldableCell cell in containedCells)
            {
                cell.OnFoldedInHierarchy(IsParentFolded);
            }
        }
    }
}

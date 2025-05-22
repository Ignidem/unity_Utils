namespace UnityUtils.DynamicScrollers.Foldables
{
	public interface IFoldableCell : IScrollerCell
	{
		bool IsFolded { get; }
		bool IsParentFolded { get; }
        
		void SetFolded(bool isFolded);
		void OnParentFoldChange(bool parentFolded);
	}
}
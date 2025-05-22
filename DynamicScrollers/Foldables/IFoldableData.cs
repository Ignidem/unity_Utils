namespace UnityUtils.DynamicScrollers.Foldables
{
	public interface IFoldableData : IScrollerCellData
	{
		bool IsFolded { get; }
	}
}
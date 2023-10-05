using UnityEngine;

namespace Assets.GameAssets.Common.Layout
{
	public interface IWorldGridLayoutElement
	{
		void SetLayoutPosition(Vector3Int gridPosition, Vector3 localPosition, int index, int count);
	}
}

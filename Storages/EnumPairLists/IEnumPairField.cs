using System;

namespace UnityUtils.Storages.EnumPairLists
{
	public interface IEnumPairField
	{
		Type EnumType { get; }
		Type ValueType { get; }
		int Count { get; }

		string GetNameAt(int index);
	}
}

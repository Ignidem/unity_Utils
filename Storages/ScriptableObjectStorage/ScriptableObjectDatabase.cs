using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Storage;

namespace UnityUtils.Storages
{
	[CreateAssetMenu(fileName = nameof(ScriptableObjectDatabase), menuName = "Storage/" + nameof(ScriptableObjectDatabase))]
	public class ScriptableObjectDatabase : ScriptableObject, ISimpleDataStorage
	{
		[SerializeField] private List<StorageObject> tables;

		private Dictionary<Type, StorageObject> HashedTables
			=> _hashedTables ??= tables.ToDictionary(t => t.TableType, t => t);
		private Dictionary<Type, StorageObject> _hashedTables;

		private bool TryGetTable<TKey, TEntry>(out StorageObject<TKey, TEntry> storage)
		{
			Type type = typeof(TEntry);
			StorageObject table = HashedTables[type];
			if (table is not StorageObject<TKey, TEntry> _s)
			{
				storage = null;
				return false;
			}

			storage = _s;
			return true;
		}

		public void DeleteEntry<TKey, TEntry>(TKey key)
		{
			if (!TryGetTable(out StorageObject<TKey, TEntry> storage))
				return;

			storage.Delete(key);
		}

		public TEntry GetEntry<TKey, TEntry>(TKey key)
		{
			if (!TryGetTable(out StorageObject<TKey, TEntry> storage))
				return default;

			return storage[key];
		}

		public void PostEntry<TKey, TEntry>(TKey key, TEntry entry)
		{
			if (!TryGetTable(out StorageObject<TKey, TEntry> storage))
				return;

			storage[key] = entry;
		}
	}
}

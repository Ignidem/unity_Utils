using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Storage;

namespace UnityUtils.Storages
{
	public class ScriptableObjectDatabase : ScriptableObject, ISimpleDataStorage, IDisposable
	{
		[SerializeField] private List<StorageObject> tables;

		private Dictionary<Type, StorageObject> HashedTables
			=> _hashedTables ??= tables.ToDictionary(t => t.TableType, t => t);
		private Dictionary<Type, StorageObject> _hashedTables;

		private bool TryGetTable<TKey, TEntry>(out IStorageObject<TKey, TEntry> storage)
		{
			Type type = typeof(TEntry);
			StorageObject table = HashedTables[type];
			if (table is not IStorageObject<TKey, TEntry> _s)
			{
				storage = null;
				return false;
			}

			storage = _s;
			return true;
		}

		public void DeleteEntry<TKey, TEntry>(TKey key)
		{
			if (!TryGetTable(out IStorageObject<TKey, TEntry> storage))
				return;

			storage.Delete(key);
		}

		public TEntry GetEntry<TKey, TEntry>(TKey key)
		{
			if (!TryGetTable(out IStorageObject<TKey, TEntry> storage))
				return default;

			return storage[key];
		}

		public TEntry[] GetAll<TEntry>()
		{
			Type type = typeof(TEntry);
			StorageObject table = HashedTables[type];
			return table.GetAllAs<TEntry>();
		}

		public void PostEntry<TKey, TEntry>(TKey key, TEntry entry)
		{
			if (!TryGetTable(out IStorageObject<TKey, TEntry> storage))
				return;

			storage[key] = entry;
		}

		public void Dispose()
		{
			for (int i = 0; i < tables.Count; i++)
			{
				StorageObject table = tables[i];
				if (table is IDisposable _d)
					_d.Dispose();
			}
		}
	}
}

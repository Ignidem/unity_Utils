namespace Utils.Storage
{
	public interface ISimpleDataStorage
	{
		TEntry GetEntry<TKey, TEntry>(TKey key);
		void PostEntry<TKey, TEntry>(TKey key, TEntry entry);
		void DeleteEntry<TKey, TEntry>(TKey key);
	}
}

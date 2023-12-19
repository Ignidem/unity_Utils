using UnityEngine;
using Utilities.Conversions;
using Utils.Storage;

namespace UnityUtils.Storage.PlayerPreferences
{
	public class PlayerStorage : ISimpleDataStorage
	{
		public static T ReadValueOrDefault<T>(string key)
		{
			if (!PlayerPrefs.HasKey(key))
				return default;

			return (T)ReadValue<T>(key);
		}

		public static bool TryLoad<T>(string key, out T value)
		{
			if (!PlayerPrefs.HasKey(key))
			{
				value = default;
				return false;
			}

			value = (T)ReadValue<T>(key);
			return true;
		}

		public static void SetValue<T>(string key, T value)
		{
			void Default()
			{
				if (value.TryToJson(out string json) || value.TryConvertTo(out json))
				{
					PlayerPrefs.SetString(key, json);
					return;
				}

				Debug.LogError($"Could not convert {{ {key} : {value} }} to json or string.");
			}

			switch (value)
			{
				case string str:
					PlayerPrefs.SetString(key, str);
					break;
				case float f:
					PlayerPrefs.SetFloat(key, f);
					break;
				case int i:
					PlayerPrefs.GetInt(key, i);
					break;
				default:
					Default();
					break;
			};
		}

		private static object ReadValue<T>(string key)
		{
			T GetObject()
			{
				string str = PlayerPrefs.GetString(key);

				return str.TryParseJson(out T value) || str.TryConvertTo(out value) 
					? value 
					: default;
			}

			return default(T) switch
			{
				string => PlayerPrefs.GetString(key),
				float => PlayerPrefs.GetFloat(key),
				int => PlayerPrefs.GetInt(key),
				_ => GetObject()
			};
		}

		public void DeleteEntry<TKey, TEntry>(TKey key)
		{
			if (!key.TryConvertTo(out string strKey))
				strKey = key.ToString();

			PlayerPrefs.DeleteKey(strKey);
		}

		public TEntry GetEntry<TKey, TEntry>(TKey key)
		{
			if (!key.TryConvertTo(out string strKey))
				strKey = key.ToString();

			return ReadValueOrDefault<TEntry>(strKey);
		}

		public TEntry[] GetAll<TEntry>()
		{
			throw new System.NotImplementedException("Player Prefs Storage doe snot support this feature.");
		}

		public void PostEntry<TKey, TEntry>(TKey key, TEntry entry)
		{
			if (!key.TryConvertTo(out string strKey))
				strKey = key.ToString();

			SetValue(strKey, entry);
		}
	}
}

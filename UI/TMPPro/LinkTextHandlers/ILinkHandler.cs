using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UnityUtils.UI.TMPPro
{
	public interface ILinkHandler
	{
		void OnLinkClick(TMP_LinkInfo link);
	}

	public class UnityEventLink : ILinkHandler
	{
		[SerializeField]
		private Serialized.Dictionary<string, UnityEvent<string>> events;

		public void OnLinkClick(TMP_LinkInfo link)
		{
			string id = link.GetLinkID();
			if (events.TryGetValue(id, out UnityEvent<string> evnt))
			{
				string text = link.GetLinkText();
				evnt.Invoke(text);
			}
		}
	}
}

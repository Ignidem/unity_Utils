using System.Text;

namespace UnityUtils.UI.TMPPro
{
	public static class RichTextEx
	{
		private const string tagFormat = "<{1}={2}>{0}</{1}>";

		public static StringBuilder AppendLink(this StringBuilder sb, string text, string id)
		{
			return sb.AppendFormat(tagFormat, text, "link", id);
		}
	}
}

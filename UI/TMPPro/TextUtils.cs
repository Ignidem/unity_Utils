using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.External.unity_utils.UI.TMPPro
{
	public static class TextUtils
	{
		public static Coroutine LoadingText(this TMP_Text label, string content)
		{
			return label.StartCoroutine(LoadingTextCoroutine(label, content));
		}
		private static IEnumerator LoadingTextCoroutine(TMP_Text label, string content)
		{
			int dotCount = 0;
			WaitForSeconds delay = new WaitForSeconds(0.5f);

			while (true)
			{
				yield return delay;
				dotCount = (dotCount + 1) % 4;
				label.text = content + new string('.', dotCount);
			}
		}
	}
}

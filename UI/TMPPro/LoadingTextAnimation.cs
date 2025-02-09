using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityUtils.GameObjects;

namespace UnityUtils.UI.TMPPro
{
	public class LoadingTextAnimation : ICoroutineHandler
	{
		public readonly TMP_Text label;

		public string content;
		public char paddingCharacter;
		public Range charCount;

		private Coroutine coroutine;

		public LoadingTextAnimation(TMP_Text label, string content, char paddingChar = '.')
		{
			this.label = label;
			this.content = content;
			this.paddingCharacter = paddingChar;
			charCount = new Range(1, 3);
			Start();
		}

		public void Start()
		{
			if (!label.isActiveAndEnabled) return;

			coroutine ??= label.StartCoroutine(LoadingTextCoroutine());
		}

		public void Stop()
		{
			if (coroutine != null)
			{
				label.StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		private IEnumerator LoadingTextCoroutine()
		{
			int dotCount = 0;
			WaitForSeconds delay = new WaitForSeconds(0.5f);

			while (true)
			{
				yield return delay;
				dotCount = (dotCount + charCount.Start.Value) % (charCount.End.Value + 1);
				label.text = content + new string(paddingCharacter, dotCount);
			}
		}
	}
}

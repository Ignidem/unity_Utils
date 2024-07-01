using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Storages.EnumPairLists;

namespace UnityUtils.UI.Selectable
{
	[System.Serializable]
	public class RotateGraphic : IButtonAnimations
	{
		[SerializeField] private Graphic target;
		[SerializeField] private EnumPair<ButtonState, float> speeds;

		private ButtonState activeState;
		private Coroutine tween;

		public void DoStateTransition(ButtonState state, bool animate)
		{
			activeState = state;
			if (tween == null)
			{
				tween = target.StartCoroutine(RotateCoroutine(this));
			}
		}

		private static IEnumerator RotateCoroutine(RotateGraphic handler)
		{
			Transform transform = handler.target.transform;
			float speed = handler.speeds[handler.activeState];
			while (handler.target && speed != 0)
			{
				Vector3 eulers = transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler(eulers + new Vector3(0, 0, speed * Time.deltaTime));

				yield return null;
				speed = handler.speeds[handler.activeState];
			}

			handler.tween = null;
		}
	}
}

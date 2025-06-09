using System;
using UnityEngine;

namespace UnityUtils.Common.Layout
{
	[ExecuteInEditMode]
	public abstract class LayoutController : MonoBehaviour
	{
		[SerializeField] private bool isReloadRequested;

		[Header("Reload On Events")]
		[SerializeField] private bool reloadOnValidate = true;
		[SerializeField] private bool reloadOnEnable = true;
		[SerializeField] private bool reloadOnChildrenChange = true;
		[SerializeField] private bool reloadOnRectChange = false;
		[Header("Animation Parameters")]
		[SerializeField] private bool animateOnAutoReload;

		protected virtual void OnValidate()
		{
			if (reloadOnValidate)
				isReloadRequested = true;
		}
		protected virtual void OnEnable()
		{
			if (reloadOnEnable)
				isReloadRequested = true;
		}
		protected virtual void OnTransformChildrenChanged()
		{
			if (reloadOnChildrenChange)
				isReloadRequested = true;
		}

		private void OnRectTransformDimensionsChange()
		{
			if (reloadOnRectChange)
				isReloadRequested = true;
		}

		protected virtual void Update()
		{
			if (isReloadRequested)
				ReloadLayout(animateOnAutoReload);
		}

		public virtual void ReloadLayout(bool animate = false)
		{
			isReloadRequested = false;
		}
	}
}

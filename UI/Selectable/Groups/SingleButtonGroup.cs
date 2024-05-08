﻿using System;
using UnityEngine;

namespace UnityUtils.UI.Selectable.Groups
{
	[Serializable]
	public class SingleButtonGroup : ISingleSelectableGroup
	{
		public delegate void SelectionChangedDelegate();

		[SerializeField]
		private Button[] buttons;

		public event SelectionChangedDelegate OnSelectionChanged;

		public ISelectableInput ActiveInput { get; private set; }

		public void Init()
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				Button button = buttons[i];
				button.Group = this;
			}
		}

		public void Select(ISelectableInput input)
		{
			if (input == ActiveInput) return;

			if (ActiveInput != null)
				ActiveInput?.OnGroupDeselected();
			ActiveInput = input;
			ActiveInput?.OnGroupSelected();
			OnSelectionChanged?.Invoke();
		}

		public void DeselectActive() => Deselect(ActiveInput);

		public void Deselect(ISelectableInput input)
		{
			if (input != ActiveInput) return;
			ActiveInput?.OnGroupDeselected();
			ActiveInput = null;
			OnSelectionChanged?.Invoke();
		}
	}
}

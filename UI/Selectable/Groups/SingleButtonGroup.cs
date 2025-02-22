using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Collections;

namespace UnityUtils.UI.Selectable.Groups
{
	[Serializable]
	public class SingleButtonGroup : ISingleSelectableGroup
	{
		public delegate void SelectionChangedDelegate();

		public Button this[int id]
		{
			get
			{
				int index = buttons.IndexOf(b => b.Id == id);
				return index == -1 ? null : buttons[index];
			}
		}

		public int Count => buttons.Count;

		[SerializeField]
		private List<Button> buttons;

		[SerializeField]
		private bool canDeselect = true;

		public event SelectionChangedDelegate OnSelectionChanged;

		public ISelectableInput ActiveInput { get; private set; }

		public void Init()
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				Button button = buttons[i];
				button.Group = this;
			}
		}

		public void Add(Button input)
		{
			if (buttons.Contains(input))
				return;

			buttons.Add(input);
			input.Group = this;
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
			if (!canDeselect || input != ActiveInput) return;
			ActiveInput?.OnGroupDeselected();
			ActiveInput = null;
			OnSelectionChanged?.Invoke();
		}
	}
}

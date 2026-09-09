using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Collections;

namespace UnityUtils.UI.Selectable.Groups
{
	[Serializable]
	public class SingleButtonGroup : SingleButtonGroup<Button> { }
	
	[Serializable]
	public class SingleButtonGroup<T> : ISingleSelectableGroup
		where T : IGroupedInput
	{
		public delegate void SelectionChangedDelegate();

		public T this[int id]
		{
			get
			{
				int index = buttons.IndexOf(b => b.Id == id);
				return index == -1 ? default : buttons[index];
			}
		}

		public int Count => buttons.Count;

		[SerializeField]
		private List<T> buttons;

		[SerializeField]
		private bool canDeselect = true;

		public event SelectionChangedDelegate OnSelectionChanged;

		public T ActiveInput { get; private set; }
		ISelectableInput ISingleSelectableGroup.ActiveInput => ActiveInput;

		public void Init()
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				T button = buttons[i];
				button.Group = this;
			}
		}

		public T At(int index)
		{
			return buttons[index];
		}
		
		public void Add(T input)
		{
			if (buttons.Contains(input))
				return;

			buttons.Add(input);
			input.Group = this;
		}

		public void Select(ISelectableInput input)
		{
			if (input is T _input)
			{
				Select(_input);
			}
			else if (input.Id >= 0 && input.Id < buttons.Count)
			{
				Select(this[input.Id]);
			}
		}
		public void Select(T input)
		{
			if (ReferenceEquals(input, ActiveInput)) return;

			if (ActiveInput != null)
				ActiveInput?.OnGroupDeselected();

			ActiveInput = input;
			ActiveInput?.OnGroupSelected();
			OnSelectionChanged?.Invoke();
		}

		public void DeselectActive() => Deselect(ActiveInput);

		public void Deselect(ISelectableInput input)
		{
			if (!canDeselect || !ReferenceEquals(input, ActiveInput)) return;
			ActiveInput?.OnGroupDeselected();
			ActiveInput = default;
			OnSelectionChanged?.Invoke();
		}
	}
}

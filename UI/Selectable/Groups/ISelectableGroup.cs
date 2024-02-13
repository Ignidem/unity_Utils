using System.Collections.Generic;

namespace UnityUtils.UI.Selectable.Groups
{
	public interface ISelectableGroup
	{
		bool TryGetActive(int i, out ISelectableInput input);
		bool IsActive(int i);
		void Toggle(ISelectableInput input)
		{
			if (IsActive(input.Id))
			{
				Deselect(input);
			}
			else
			{
				Select(input);
			}
		}
		void Select(ISelectableInput input);
		void Deselect(ISelectableInput input);
		void Deselect(int i)
		{
			if (TryGetActive(i, out ISelectableInput input))
				Deselect(input);
		}
	}

	public interface ISingleSelectableGroup : ISelectableGroup
	{
		ISelectableInput ActiveInput { get; }

		bool ISelectableGroup.IsActive(int i)
		{
			return ActiveInput?.Id == i;
		}
		bool ISelectableGroup.TryGetActive(int i, out ISelectableInput input)
		{
			if (IsActive(i))
			{
				input = ActiveInput;
				return true;
			}

			input = null;
			return false;
		}
		void DeselectActive() => Deselect(ActiveInput);
	}

	public interface IMultiSelectableGroup : ISelectableGroup
	{
		Dictionary<int, ISelectableInput> ActivesInputs { get; }
		bool ISelectableGroup.TryGetActive(int i, out ISelectableInput input)
		{
			return ActivesInputs.TryGetValue(i, out input);
		}
		void ISelectableGroup.Select(ISelectableInput input)
		{
			if (IsActive(input.Id))
				return;

			ActivesInputs.Add(input.Id, input);
			input.OnGroupSelected();
		}
		void ISelectableGroup.Deselect(ISelectableInput input)
		{
			ActivesInputs.Remove(input.Id);
			input.OnGroupDeselected();
		}
		void DeselectAllActive()
		{
			foreach (ISelectableInput input in ActivesInputs.Values)
			{
				Deselect(input);
			}
		}
	}
}

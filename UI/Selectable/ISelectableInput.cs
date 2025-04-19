using UnityUtils.UI.Selectable.Groups;

namespace UnityUtils.UI.Selectable
{
	public interface IGroupedInput : ISelectableInput
	{
		ISelectableGroup Group { get; set; }
	}
	
	public interface ISelectableInput
	{
		event OnPointerDelegate PointerDownEvent;
		event OnPointerDelegate PointerUpEvent;
		event OnPointerDelegate PointerEnterEvent;
		event OnPointerDelegate PointerExitEvent;
		event OnPointerDelegate PointerPressEvent;
		int Id { get; }
		bool IsToggle { get; }

		void OnGroupSelected();
		void OnGroupDeselected();
	}
}

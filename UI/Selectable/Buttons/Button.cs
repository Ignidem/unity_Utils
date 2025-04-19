using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtils.PropertyAttributes;
using UnityUtils.UI.ImageComponents;
using UnityUtils.UI.Selectable.Groups;
using Utils.Logger;

namespace UnityUtils.UI.Selectable
{
	public delegate void OnPointerDelegate(PointerEventData data);

	public class Button : UnityEngine.UI.Button, IGroupedInput
	{
		public event OnPointerDelegate PointerDownEvent;
		public event OnPointerDelegate PointerUpEvent;
		public event OnPointerDelegate PointerEnterEvent;
		public event OnPointerDelegate PointerExitEvent;
		public event OnPointerDelegate PointerPressEvent;

		[field: SerializeField]
		public int Id { get; set; }

		[field: SerializeReference, Polymorphic(true)]
		public ISelectableGroup Group { get; set; }

		[field: SerializeField]
		public bool IsToggle { get; private set; }
		public bool IsToggleSelected
		{
			get => actAsSelected;
			set
			{
				actAsSelected = value;
				ReloadAnimation(false);
			}
		}
		public bool IsOn => actAsSelected || IsActiveGroupInput;

		public bool HasGroup => Group != null;
		public bool IsActiveGroupInput => Group?.IsActive(Id) ?? false;
		public bool actAsSelected;

		[SerializeField] private TMP_Text label;
		public string Text
		{
			get => label ? label.text : null;
			set
			{
				if (label) label.text = value;
			}
		}

		[field: SerializeReference, Polymorphic]
		public IImageComponent Icon { get; private set; }

		[SerializeReference, Polymorphic]
		[InspectorName("Events")]
		private IButtonAnimations[] animations;

		protected override void Start()
		{
			base.Start();
			ReloadAnimation(true);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			PointerDownEvent = null;
			PointerUpEvent = null;
			PointerEnterEvent = null;
			PointerExitEvent = null;
			PointerPressEvent = null;
			onClick.RemoveAllListeners();
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			PointerDownEvent?.Invoke(eventData);
		}
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			PointerUpEvent?.Invoke(eventData);
		}
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			PointerEnterEvent?.Invoke(eventData);
			//force highlight, when "selected" base ignores state change; smh
			UpdateAnimations(ButtonState.Highlighted, true);
		}
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			PointerExitEvent?.Invoke(eventData);
		}
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (IsToggle)
				actAsSelected = !actAsSelected;

			base.OnPointerClick(eventData);
			PointerPressEvent?.Invoke(eventData);
			Group?.Toggle(this);
		}
		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			ReloadAnimation(state, instant);
		}

		public void ReloadAnimation(bool instant)
		{
			ReloadAnimation(currentSelectionState, instant);
		}

		private void ReloadAnimation(SelectionState state, bool instant)
		{
			ButtonState bntState = GetButtonState(state);
			UpdateAnimations(bntState, !instant);
		}

		public ButtonState GetButtonState() => GetButtonState(currentSelectionState);
		private ButtonState GetButtonState(SelectionState state)
		{
			if (IsActiveGroupInput)
				return HighlightedOrState(ButtonState.GroupSelected);

			if (actAsSelected)
				return HighlightedOrState(ButtonState.Selected);

			return state switch
			{
				SelectionState.Highlighted => ButtonState.Highlighted,
				SelectionState.Pressed => ButtonState.Pressed,
				//If the button is actually selected, one of the previous if cases would be true;
				SelectionState.Selected => HighlightedOrState(!IsOn ? ButtonState.Normal : ButtonState.Selected),
				SelectionState.Disabled => ButtonState.Disabled,
				SelectionState.Normal => HasGroup ? ButtonState.GroupDeselected : ButtonState.Normal,
				_ => throw new System.ArgumentOutOfRangeException(),
			};
		}
		private ButtonState HighlightedOrState(ButtonState state)
		{
			return IsToggle || !this.IsPointerInside() ? state : ButtonState.Highlighted;
		}

		public virtual void OnGroupSelected() 
		{
			UpdateAnimations(ButtonState.GroupSelected, true);
		}
		public virtual void OnGroupDeselected() 
		{
			UpdateAnimations(ButtonState.GroupDeselected, true);
		}

		private void UpdateAnimations(ButtonState state, bool animate)
		{
			if (animations == null)
				return;

			for (int i = 0; i < animations.Length; i++)
			{
				try
				{
					IButtonAnimations animation = animations[i];
					animation?.DoStateTransition(state, animate);
				}
				catch (Exception e)
				{
					Debug.LogError($"Exception caught in {name}", gameObject);
					e.LogException();
				}
			}
		}
	}
}

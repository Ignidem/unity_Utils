using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener.CallbackMiddleware
{
	/// <summary>
	/// Mouse Delta Input Composite Middleware used to handle ui and certain 3d object raycast blocks to ignore the input.
	/// Currently only supports OneModifier Composites with a Button Modifier and a Mouse Delta input.
	/// </summary>
	public class MouseDeltaComposite : ICallbackMiddleware<Vector2>
	{
		private static readonly LayerMask uiLayerMask = LayerMask.GetMask("UI");
		
		private readonly InputAction action;
		
		private readonly int compositeIndex;
		private readonly int buttonIndex;
		private readonly int deltaIndex;

		private InputBinding ButtonBinding => action.bindings[buttonIndex];
		private InputBinding DeltaBinding => action.bindings[deltaIndex];
		
		private InputControl ButtonControl
		{
			get
			{
				InputBinding binding = ButtonBinding;
				if (_buttonControl == null || InputControlPath.Matches(binding.effectivePath, _buttonControl))
					_buttonControl = action.controls.FirstOrDefault(c => InputControlPath.Matches(binding.effectivePath, c));

				return _buttonControl;
			}
		}
		private InputControl _buttonControl;
		
		private bool wasButtonPressed;
		private bool isModiferOngoing;

		public MouseDeltaComposite(InputAction action)
		{
			this.action = action;
			compositeIndex = GetCompositeIndex(out buttonIndex, out deltaIndex);
		}

		private int GetCompositeIndex(out int buttonIndex, out int deltaIndex)
		{
			const string modifier = nameof(modifier);
			for (int i = 0; i < action.bindings.Count - 2; i++)
			{
				InputBinding composite = action.bindings[i];
				if (!composite.isComposite) continue;
				
				InputBinding component1 = action.bindings[i + 1];
				InputBinding component2 = action.bindings[i + 2];

				if (component1.name == modifier && IsMouseDeltaBinding(component2))
				{
					buttonIndex = i + 1;
					deltaIndex = i + 2;
					return i;
				}

				if (component2.name == modifier && IsMouseDeltaBinding(component1))
				{
					buttonIndex = i + 2;
					deltaIndex = i + 1;
					return i;
				}
			}

			buttonIndex = -1;
			deltaIndex = -1;
			return -1;
		}
		private static bool IsMouseDeltaBinding(InputBinding component)
		{
			const string binding = nameof(binding);
			const string delta = "<Mouse>/delta";
			return component is { name: binding, effectivePath: delta };
		}
		
		private static bool ActiveInterfaceInteraction()
		{
			EventSystem system = EventSystem.current;
			if (!system) return false;
			
			PointerEventData eventData = new(EventSystem.current)
			{
				position = Input.mousePosition
			};

			List<RaycastResult> results = new();
			EventSystem.current.RaycastAll(eventData, results);
			
			return results.Any(IsControlBlockingRaycast);
		}

		private static bool IsControlBlockingRaycast(RaycastResult result)
		{
			GameObject go = result.gameObject;
			if ((1 << go.layer) == uiLayerMask) return true;

			return go.GetComponent<IDragHandler>() != null;
		}
		
		/// <summary>
		/// Updates the button states based on input button pressed state and UI presence.
		/// </summary>
		/// <returns>Was the state changed.</returns>
		private bool UpdateButtonState()
		{
			//Avoiding checking if UI has active interaction every frame
			//by only checking when the button was first pressed.
			if (wasButtonPressed)
			{
				if (ButtonControl.IsPressed())
					return false;
				
				wasButtonPressed = false;
				isModiferOngoing = false;
				return true;
			}

			if (!ButtonControl.IsPressed()) return false;
			
			wasButtonPressed = true;
			isModiferOngoing = !ActiveInterfaceInteraction();
			return true;

		}
		private bool IsActiveDeltaControl()
		{
			return action.activeControl != null && 
			       InputControlPath.Matches(DeltaBinding.effectivePath, action.activeControl);
		}

		private bool IgnoreControl()
		{
			if (compositeIndex == -1) return false;
			//If the state was changed, we want to ignore the control to ignore the first frame
			if (UpdateButtonState()) return true;
			return !isModiferOngoing && IsActiveDeltaControl();
		}
		
		public bool InvokeActionValue(out Vector2 value)
		{
			if (IgnoreControl())
			{
				value = Vector2.zero;
				return false;
			}
			
			value = action.ReadValue<Vector2>();
			return true;
		}
		
		public bool InvokeContextValue(InputAction.CallbackContext context, out Vector2 value)
		{
			if (IgnoreControl())
			{
				value = Vector2.zero;
				return false;
			}
			
			value = context.ReadValue<Vector2>();
			return true;
		}
	}
}
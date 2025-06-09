#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace UnityUtils.CSharpInputListener
{
	public static class InputActionUtils
	{
		public static int GetBindingIndex(this InputAction.CallbackContext context)
		{
			return context.action.GetBindingIndexForControl(context.control);
		}

		public static InputBinding GetBinding(this InputAction.CallbackContext context)
		{
			int index = context.GetBindingIndex();
			return context.action.bindings[index];
		}

		public static bool IsContinuous(this InputAction.CallbackContext context)
		{
			return context.control.name is "delta" or "position" or "scroll";
		}
		public static bool IsModifier(this InputBinding binding)
		{
			return binding is { isPartOfComposite: true, name: "modifier" };
		}

		private delegate IEnumerable GetPointerStatesDelegate(InputSystemUIInputModule module);
		private delegate ExtendedPointerEventData GetEventDataDelegate(object pointerStates);

		private static readonly GetPointerStatesDelegate GetPointerStates = CompileGetPointerDataFunction();
		private static readonly GetEventDataDelegate GetEventData = CompileEventDataFunction();

		private static GetPointerStatesDelegate CompileGetPointerDataFunction()
		{
			Type moduleType = typeof(InputSystemUIInputModule);
			FieldInfo field = moduleType.GetField("m_PointerStates", BindingFlags.NonPublic | BindingFlags.Instance);
			if (field == null)
			{
				throw new Exception("Unable to find 'm_PointerStates'.");
			}
			
			ParameterExpression instanceParam = Expression.Parameter(moduleType, "module");
			MemberExpression fieldAccess = Expression.Field(instanceParam, field);
			UnaryExpression convert = Expression.Convert(fieldAccess, typeof(IEnumerable));
			return Expression.Lambda<GetPointerStatesDelegate>(convert, instanceParam).Compile();
		}
		private static GetEventDataDelegate CompileEventDataFunction()
		{
			Type pointerModelType = typeof(InputSystemUIInputModule).Assembly
				.GetType("UnityEngine.InputSystem.UI.PointerModel") 
			    ?? throw new Exception("Unable to find 'PointerModel' type.");
			
			FieldInfo eventDataField = pointerModelType.GetField("eventData", BindingFlags.Public | BindingFlags.Instance)
				?? throw new Exception("Unable to find 'eventData' field on PointerModel.");
			
			ParameterExpression pointerModelParam = Expression.Parameter(typeof(object), "pointerModel");
			UnaryExpression castPointerModel = Expression.Convert(pointerModelParam, pointerModelType);
			MemberExpression eventDataAccess = Expression.Field(castPointerModel, eventDataField);
			UnaryExpression castEventData = Expression.Convert(eventDataAccess, typeof(ExtendedPointerEventData));
			return Expression.Lambda<GetEventDataDelegate>(castEventData, pointerModelParam).Compile();
		}

		public static IEnumerable<ExtendedPointerEventData> GetPointerData(this InputSystemUIInputModule module)
		{
			IEnumerable pointerStates = GetPointerStates(module);
			foreach (object state in pointerStates)
			{
				yield return GetEventData(state);
			}
		}
	}
}
#endif
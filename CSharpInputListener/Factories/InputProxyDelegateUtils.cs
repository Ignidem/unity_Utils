#if ENABLE_INPUT_SYSTEM
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine;
using Utilities.Collections;
using System;
using UnityEngine.InputSystem.Controls;

namespace UnityUtils.CSharpInputListener
{
	public static class InputProxyDelegateUtils
	{
		private static readonly ActionInputInjectorFactory factory = new(typeof(InputActionContextInjector<>));
		
		public static Constructor GetInjectorConstructor(this InputAction action)
		{
			return factory.GetConstructor(action);
		}
		
		public static Type GetExpectedValueType(this InputAction action)
		{
			return action.type switch
			{
				InputActionType.Button => typeof(float),
				InputActionType.Value or InputActionType.PassThrough => action.expectedControlType switch
				{
					"Axis" or "Analog" or "Button" => typeof(float),
					"Vector2" or "Stick" or "Dpad" => typeof(Vector2),
					"Vector3" => typeof(Vector3),
					"Quaternion" => typeof(Quaternion),
					"Integer" => typeof(int),
					"Double" => typeof(double),
					"Key" => typeof(Key),
					"Touch" => typeof(TouchControl),
					_ => null
				},
				_ => null
			};
		}
	}
}
#endif
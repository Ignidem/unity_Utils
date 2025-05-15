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
		private static readonly ActionInputInjectorFactory[] factories = new[]
		{
			new ActionInputInjectorFactory(typeof(ActionInputInjector<>), typeof(ActionInputInjector<>.Callback)),
			new ActionInputInjectorFactory(typeof(ActionContextInjector<>), typeof(ActionContextInjector<>.Callback)),
		};
		
		public static IActionInputInjector CreateInjector(this IInputReceiver receiver, 
			InputAction action, MethodInfo method)
		{
			CallbackInfo info = new CallbackInfo(receiver, action, method);

			for (var i = 0; i < factories.Length; i++)
			{
				ActionInputInjectorFactory factory = factories[i];
				if (factory.TryCreate(info, out IActionInputInjector injector))
					return injector;
			}

			throw new Exception($"Method {method.Name} definition is not supported by any action injectors.");
		}

		private static IActionInputInjector CreateFromDefinedType(Type definedType,
			IInputReceiver receiver, InputAction action, MethodInfo method)
		{
			return Activator.CreateInstance(definedType, receiver, action, method) as IActionInputInjector;
		}
		
		private static bool ValidateParametersPosition(ParameterInfo[] parameters, out int inputIndex, out int contextIndex)
		{
			switch (parameters.Length)
			{
				case 1:
					Type type = parameters[0].ParameterType;
					contextIndex = type == typeof(InputAction.CallbackContext) ? 0 : -1;
					inputIndex = contextIndex != -1 ? -1 : 0;
					return true;
				case 2:
					contextIndex = parameters.IndexOf(p => p.ParameterType == typeof(InputAction.CallbackContext));
					if (contextIndex == -1)
					{
						inputIndex = -1;
						return false;
					}
					inputIndex = (contextIndex + 1) % 2;
					return true;
				default:
					inputIndex = -1;
					contextIndex = -1;
					return false;
			}
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
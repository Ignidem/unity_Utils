#if ENABLE_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.InputSystem;
using Utils.Delegates;

namespace Assets.External.unity_utils.CSharpInputListener
{
	public interface IInputReceiver { }

	public class InputProxy
	{
		private const BindingFlags methodReceiversFlags = BindingFlags.Public | BindingFlags.Instance | 
			BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly;

		private static readonly Type genericType = typeof(InputReceiverMethod<>);
		private static readonly Dictionary<Type, Type> definedTypes = new();

		private static IInputReceiverMethod CreateReceiverMethod(MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			if (parameters.Length != 1) return null;
			Type inputType = parameters[0].ParameterType;
			if (inputType.IsClass || inputType.IsInterface) return null;

			if (!definedTypes.TryGetValue(inputType, out Type definedType))
			{
				definedType = genericType.MakeGenericType(inputType);
				definedTypes.Add(inputType, definedType);
			}

			return Activator.CreateInstance(definedType, new object[] { method }) as IInputReceiverMethod;
		}

		public bool IsActive
		{
			get => inputs.isActiveAndEnabled && inputs.inputIsActive;
			set
			{
				inputs.enabled = value;
			}
		}

		private readonly PlayerInput inputs;
		private readonly IInputReceiver receiver;
		private readonly Dictionary<string, IInputReceiverMethod> receivers;

		public InputProxy(PlayerInput inputs, IInputReceiver receiver)
		{
			inputs.onActionTriggered += OnAction;
			receivers = new();
			this.inputs = inputs;
			this.receiver = receiver;
			receiver.ForeachMethodWithAttribute<InputReceiverAttribute>(ParseMethod, methodReceiversFlags);
		}

		private void ParseMethod(MethodInfo method, InputReceiverAttribute attribute)
		{
			IInputReceiverMethod receiverMethod = CreateReceiverMethod(method);
			if (receiverMethod == null) return;

			receivers.Add(attribute.name, receiverMethod);
		}

		private void OnAction(InputAction.CallbackContext obj)
		{
			if (!IsActive) return;

			if (!receivers.TryGetValue(obj.action.name, out IInputReceiverMethod method))
			{
				return;
			}

			method.Invoke(receiver, obj.action);
		}
	}
}
#endif
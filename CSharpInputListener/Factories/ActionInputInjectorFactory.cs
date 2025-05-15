#if ENABLE_INPUT_SYSTEM
using System;
using System.Linq.Expressions;
using System.Reflection;
using Serialized;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class ActionInputInjectorFactory
	{
		private static readonly ParameterExpression[] constructorParameters = new ParameterExpression[]
		{
			Expression.Parameter(typeof(IInputReceiver), "receiver"),
			Expression.Parameter(typeof(InputAction), "action"),
			Expression.Parameter(typeof(MethodInfo), "method")
		};
		
		private readonly Type genericType;
		private readonly MethodInfo callbackInvoke;
		private readonly Dictionary<Type, Constructor> factories;

		public ActionInputInjectorFactory(Type genericType, Type callbackDelegateType)
		{
			this.genericType = genericType;
			callbackInvoke = callbackDelegateType?.GetMethod("Invoke");
			factories = new Dictionary<Type, Constructor>();
			ValidateConstructor(genericType);
		}

		private static void ValidateConstructor(Type genericType)
		{
			var constructors = genericType.GetConstructors();
			if (constructors.Length != 1) 
				throw new Exception("Single constructor required for type " + genericType.FullName);
			ConstructorInfo constructor = constructors[0];
			ParameterInfo[] parameters = constructor.GetParameters();

			if (parameters.Length != constructorParameters.Length)
				throw new Exception(constructorParameters.Length + " parameters required for constructor for type " + genericType.FullName);

			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterExpression parameter = constructorParameters[i];
				if (parameters[i].ParameterType != parameter.Type)
					throw new Exception($"Expected {parameter} type for parameter {i} of constructor " + genericType.FullName);
			}
		}

		public bool TryCreate(CallbackInfo info, out IActionInputInjector injector)
		{
			if (!ValidateInfo(info))
			{
				injector = null;
				return false;
			}
			
			if (!factories.TryGetValue(info.InputType, out Constructor constructor))
			{
				constructor = CreateConstructor(info.InputType);
				factories[info.InputType] = constructor;
			}

			injector = constructor(info.receiver, info.action, info.method);
			return true;
		}

		protected virtual bool ValidateInfo(CallbackInfo info)
		{
			if (callbackInvoke == null)
				return true;

			ParameterInfo[] delegateParams = callbackInvoke.GetParameters();
			if (delegateParams.Length != info.parameters.Length)
				return false;

			for (int i = 0; i < delegateParams.Length; i++)
			{
				Type delegateParameter = delegateParams[i].ParameterType;
				if (delegateParameter.IsGenericParameter)
				{
					delegateParameter = info.action.GetExpectedValueType();
				}

				if (info.parameters[i].ParameterType != delegateParameter)
					return false; 
			}

			return callbackInvoke.ReturnType == info.method.ReturnType;
		}
		
		private Constructor CreateConstructor(Type argType)
		{
			//IInputReceiver receiver, InputAction action, MethodInfo method
			Type type = genericType.MakeGenericType(argType);
			ConstructorInfo ctor = type.GetConstructors()[0];
			
			// Parameters of the delegate
			NewExpression newExpr = Expression.New(ctor, constructorParameters[0], constructorParameters[1], constructorParameters[2]);
			UnaryExpression convert = Expression.Convert(newExpr, typeof(IActionInputInjector));

			// Compile into a delegate
			var lambda = Expression.Lambda<Constructor>(convert, constructorParameters);
			return lambda.Compile();
		}
	}
}
#endif
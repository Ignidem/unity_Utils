#if ENABLE_INPUT_SYSTEM
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Serialized;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class ActionInputInjectorFactory
	{
		private static readonly ParameterExpression[] constructorParameters = typeof(Constructor).GetMethod("Invoke")!
			.GetParameters().Select(p => Expression.Parameter(p.ParameterType)).ToArray();
		
		private readonly Type genericType;
		private readonly Dictionary<Type, Constructor> factories;

		public ActionInputInjectorFactory(Type genericType)
		{
			this.genericType = genericType;
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

		public Constructor GetConstructor(InputAction action)
		{
			Type inputType = action.GetExpectedValueType();
			if (!factories.TryGetValue(inputType, out Constructor constructor))
			{
				constructor = CreateConstructor(inputType);
				factories[inputType] = constructor;
			}

			return constructor;
		}
		
		private Constructor CreateConstructor(Type argType)
		{
			//IInputReceiver receiver, InputAction action, MethodInfo method
			Type type = genericType.MakeGenericType(argType);
			ConstructorInfo ctor = type.GetConstructors()[0];
			
			// Parameters of the delegate
			
			NewExpression newExpr = Expression.New(ctor, constructorParameters.Select(e => (Expression)e).ToArray());
			UnaryExpression convert = Expression.Convert(newExpr, typeof(IActionInputHandler));

			// Compile into a delegate
			var lambda = Expression.Lambda<Constructor>(convert, constructorParameters);
			return lambda.Compile();
		}
	}
}
#endif
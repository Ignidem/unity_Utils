using System;
using System.Linq.Expressions;
using System.Reflection;

namespace UnityUtils.UI.Selectable
{
	public static class ButtonUtils
	{
		private static Predicate<UnityEngine.UI.Selectable> _isPointerInside;
		public static bool IsPointerInside(this UnityEngine.UI.Selectable button)
		{
			if (_isPointerInside == null)
			{
				const string fieldName = "isPointerInside";
				Type type = typeof(UnityEngine.UI.Selectable);
				PropertyInfo fieldInfo = type.GetProperty(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
					?? throw new ArgumentException($"Field '{fieldName}' not found in type '{type.Name}'.");

				ParameterExpression parameter = Expression.Parameter(type, "obj");
				MemberExpression fieldAccess = Expression.Property(parameter, fieldInfo);
				var lambda = Expression.Lambda<Predicate<UnityEngine.UI.Selectable>>(fieldAccess, parameter);
				_isPointerInside = lambda.Compile();
			}

			return _isPointerInside(button);
		}
	}
}

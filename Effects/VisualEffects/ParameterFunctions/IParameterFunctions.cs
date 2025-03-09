namespace UnityUtils.Effects.VisualEffects.ParameterFunctions
{
	public interface IParameterFunctions<TComponent>
	{
		T GetValue<T>(TComponent component, int id);
		void SetValue<T>(TComponent component, int id, T value, bool isOptional = false);
	}
}

using Utils.EventSystem;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IVisualEffectEventHandler : ISimpleEventHandler<object> { }

	public enum VisualEffectEvents
	{
		UpdateParameters
	}
}

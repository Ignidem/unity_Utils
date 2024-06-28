using Utils.EventSystem;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IVisualEffectEventHandler : IEventHandler<object> { }

	public enum VisualEffectEvents
	{
		UpdateParameters
	}
}

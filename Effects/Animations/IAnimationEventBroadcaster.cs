using System.Threading.Tasks;
using UnityUtils.Animations.AnimationEvents;

namespace UnityUtils.Animations.StateListener
{
	public delegate void AnimationEventDelegate(string subEvent, IAnimationEventInfo evnt);

	public interface IAnimationEventBroadcaster
	{
		public event AnimationEventDelegate OnEvent;
		public void Broadcast(string subEvent, IAnimationEventInfo info);
		Task<bool> OnEventAsync(string eventName, float offset);
	}
}

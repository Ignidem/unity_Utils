#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class MapInputProxy<T> : InputProxy<T>
		where T : IInputReceiver
	{
		private readonly InputActionMap map;

		public MapInputProxy(InputActionMap map, T receiver, ActionDelegateMap<T> delegateMap)
			: base(receiver, delegateMap)
		{
			this.map = map;
		}
		
		public override void Enable()
		{
			map.actionTriggered += OnAction;
		}

		public override void Disable()
		{
			map.actionTriggered -= OnAction;
		}
	}
}
#endif
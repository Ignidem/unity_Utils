#if ENABLE_INPUT_SYSTEM

namespace UnityUtils.CSharpInputListener
{
	public class ActionInputProxy<T> : InputProxy<T>
		where T : IInputReceiver
	{
		public ActionInputProxy(T receiver, ActionDelegateMap<T> map) 
			: base(receiver, map) { }
		
		public override void Enable()
		{
		}

		public override void Disable()
		{
		}
	}
}
#endif
#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class PlayerInputProxy<T> : InputProxy<T>
		where T : IInputReceiver
	{
		protected override bool IsActive => base.IsActive && inputs.isActiveAndEnabled && inputs.inputIsActive;

		private readonly PlayerInput inputs;

		public PlayerInputProxy(PlayerInput inputs, T receiver, ActionDelegateMap<T> map) 
			: base(receiver, map)
		{
			this.inputs = inputs;
		}

		public override void Enable()
		{
			inputs.onActionTriggered += OnAction;
		}

		public override void Disable()
		{
			inputs.onActionTriggered -= OnAction;
		}
	}
}
#endif
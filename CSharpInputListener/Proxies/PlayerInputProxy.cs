#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;

namespace UnityUtils.CSharpInputListener
{
	public class PlayerInputProxy : InputProxy
	{
		public override bool IsActive
		{
			get => active && receiver.IsActive && inputs.isActiveAndEnabled && inputs.inputIsActive;
			set => active = value;
		}
		
		private readonly PlayerInput inputs;
		private bool active = true;

		public PlayerInputProxy(PlayerInput inputs, IInputReceiver receiver) 
			: base(receiver, inputs.actions)
		{
			this.inputs = inputs;
			inputs.onActionTriggered += OnAction;
		}
		
		public override void Dispose()
		{
			inputs.onActionTriggered -= OnAction;
		}
		
		private void OnAction(InputAction.CallbackContext context)
		{
			if (!IsActive)
				return;
			
			if (TryGetAction(context.action, out IActionInputInjector method))
				method.Invoke(context);
		}
	}
}
#endif
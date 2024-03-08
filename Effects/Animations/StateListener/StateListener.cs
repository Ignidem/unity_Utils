using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityUtils.Animations.StateListener
{
	public class StateListener : StateMachineBehaviour, IAnimationState
	{
		[field: SerializeField]
		public string StateName { get; private set; }

		private IAnimatorHandler handler;
		private bool lookedForHandler;

		public int Id => info.shortNameHash == 0 ? Animator.StringToHash(StateName) : info.shortNameHash;
		public bool IsPlaying { get; private set; }
		public float Length => info.length;
		public int Layer { get; private set; }

		private Animator animator;
		private AnimatorStateInfo info;

		private void InitHandler(Animator animator)
		{
			if (lookedForHandler)
				return;

			lookedForHandler = true;

			this.animator = animator;

			if (handler != null)
			{
				Debug.LogWarning("Handler already exists: " + handler);
			}

			if (animator.gameObject.TryGetComponent(out IAnimatorHandler _handler))
			{
				handler = _handler;
			}
			else if (animator.gameObject.TryGetComponent(out IAnimatorHandlerProvider provider))
			{
				handler = provider.Handler;
			}

			if (handler == null)
			{
				Debug.LogWarning("No handler found");
				return;
			}

			handler.RegisterState(this);
		}

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			base.OnStateMachineEnter(animator, stateMachinePathHash);
			InitHandler(animator);
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			IsPlaying = true;
			info = stateInfo;
			this.Layer = layerIndex;
			InitHandler(animator);
			handler?.OnStateEnter(this);
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateUpdate(animator, stateInfo, layerIndex);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateExit(animator, stateInfo, layerIndex);
			IsPlaying = false;
			info = stateInfo;
			this.Layer = layerIndex;
			handler?.OnStateExit(this);
		}

		public void Play(float blendTime = 0.1F)
		{
			if (IsPlaying)
				return;

			handler.SwitchState(info.fullPathHash, Layer, blendTime);
		}

		public void Stop(float blendTime = 0.1f)
		{
			if (!IsPlaying)
				return;

			AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(Layer);
			handler.SwitchState(info.fullPathHash, Layer, blendTime);
		}

		public TaskAwaiter GetAwaiter()
		{
			if (!IsPlaying)
				return Task.CompletedTask.GetAwaiter();

			return EndAsync().GetAwaiter();
		}

		public override string ToString()
		{
			return StateName;
		}

		private async Task EndAsync()
		{
			while (IsPlaying)
				await Task.Yield();
		}
	}
}

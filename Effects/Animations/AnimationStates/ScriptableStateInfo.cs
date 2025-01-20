using System.Runtime.CompilerServices;
using UnityEngine;
using UnityUtils.Animations.AnimationEvents;
using UnityUtils.Animations.StateListener;

namespace UnityUtils.Effects.Animations.AnimationStates
{
	public class ScriptableStateInfo : ScriptableObject, IAnimationState
	{
		[field: SerializeField, HideInInspector] public int Id { get; private set; }
		[field: SerializeField]	public string StateName { get; private set; }
		[field: SerializeField]	public AnimationClip Motion { get; private set; }
		[field: SerializeField]	public float Speed { get; private set; }
		[field: SerializeField]	public bool Mirror { get; private set; }
		[field: SerializeField] public int Layer { get; private set; }

		[field: SerializeReference]
		public IAnimationTransition[] Transitions { get; private set; }

		public bool IsLoop => Motion.isLooping;
		public float Length => Motion.length;
		public float NormalizedTime => Time / Length;

		public bool IsPlaying { get; private set; }
		public float Time { get; private set; }
		public float Weight { get; private set; }

		public IAnimationEventBroadcaster EventBroadcaster { get; set; }

#if UNITY_EDITOR
		private void OnValidate()
		{
			Id = Animator.StringToHash(StateName);
		}
#endif

		#region State Methods
		public TaskAwaiter GetAwaiter()
		{
			throw new System.NotImplementedException();
		}
		public IAnimationEventInfo GetEventInfo(Animator animator, string name)
		{
			throw new System.NotImplementedException();
		}
		public void Pause()
		{
			throw new System.NotImplementedException();
		}
		public void Play(float blendTime = 0.1F)
		{
			throw new System.NotImplementedException();
		}
		public void Resume()
		{
			throw new System.NotImplementedException();
		}
		public void Stop(float blendTime = 0.1F)
		{
			throw new System.NotImplementedException();
		}
		#endregion
	}
}

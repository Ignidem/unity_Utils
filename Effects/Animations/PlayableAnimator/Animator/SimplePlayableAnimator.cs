using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	[RequireComponent(typeof(Animator))]
	public partial class SimplePlayableAnimator : MonoBehaviour
	{
		public Animator Animator
		{
			get
			{
				if (m_Animator == null)
				{
					m_Animator = GetComponent<Animator>();
				}
				return m_Animator;
			}
		}

		public bool animatePhysics
		{
			get { return m_AnimatePhysics; }
			set { m_AnimatePhysics = value; Animator.updateMode = m_AnimatePhysics ? AnimatorUpdateMode.Fixed : AnimatorUpdateMode.Normal; }
		}

		public AnimatorCullingMode cullingMode
		{
			get { return Animator.cullingMode; }
			set { m_CullingMode = value; Animator.cullingMode = m_CullingMode; }
		}

		public bool isPlaying { get { return m_Playable.IsPlaying(); } }

		public bool playAutomatically
		{
			get { return m_PlayAutomatically; }
			set { m_PlayAutomatically = value; }
		}

		public AnimationClip clip
		{
			get { return m_Clip; }
			set
			{
				LegacyClipCheck(value);
				m_Clip = value;
			}
		}

		public WrapMode wrapMode
		{
			get { return m_WrapMode; }
			set { m_WrapMode = value; }
		}

		public void AddClip(AnimationClip clip, string newName)
		{
			LegacyClipCheck(clip);
			AddState(clip, newName);
		}

		public void Blend(string stateName, float targetWeight, float fadeLength)
		{
			m_Animator.enabled = true;
			Kick();
			m_Playable.Blend(stateName, targetWeight, fadeLength);
		}

		public void CrossFade(string stateName, float fadeLength)
		{
			m_Animator.enabled = true;
			Kick();
			m_Playable.Crossfade(stateName, fadeLength);
		}

		public void CrossFadeQueued(string stateName, float fadeLength, QueueMode queueMode)
		{
			m_Animator.enabled = true;
			Kick();
			m_Playable.CrossfadeQueued(stateName, fadeLength, queueMode);
		}

		public int GetClipCount()
		{
			return m_Playable.GetClipCount();
		}

		public bool IsPlaying(string stateName)
		{
			return m_Playable.IsPlaying(stateName);
		}

		public void Stop()
		{
			m_Playable.StopAll();
		}

		public void Stop(string stateName)
		{
			m_Playable.Stop(stateName);
		}

		public void Sample()
		{
			m_Graph.Evaluate();
		}

		public bool Play()
		{
			m_Animator.enabled = true;
			Kick();
			if (m_Clip != null && m_PlayAutomatically)
			{
				m_Playable.Play(kDefaultStateName);
			}
			return false;
		}

		public void AddState(AnimationClip clip, string name)
		{
			LegacyClipCheck(clip);
			Kick();
			if (m_Playable.AddClip(clip, name))
			{
				RebuildStates();
			}

		}

		public void RemoveState(string name)
		{
			if (m_Playable.RemoveClip(name))
			{
				RebuildStates();
			}
		}

		public bool Play(string stateName)
		{
			m_Animator.enabled = true;
			Kick();
			return m_Playable.Play(stateName);
		}

		public void PlayQueued(string stateName, QueueMode queueMode)
		{
			m_Animator.enabled = true;
			Kick();
			m_Playable.PlayQueued(stateName, queueMode);
		}

		public void RemoveClip(AnimationClip clip)
		{
			if (clip == null)
				throw new System.NullReferenceException("clip");

			if (m_Playable.RemoveClip(clip))
			{
				RebuildStates();
			}

		}

		public void Rewind()
		{
			Kick();
			m_Playable.Rewind();
		}

		public void Rewind(string stateName)
		{
			Kick();
			m_Playable.Rewind(stateName);
		}

		public State GetState(string stateName)
		{
			SimpleAnimationPlayable.IState state = m_Playable.GetState(stateName);
			if (state == null)
				return null;

			return new StateImpl(state, this);
		}

		public IEnumerable<State> GetStates()
		{
			return new StateEnumerable(this);
		}

		public State this[string name]
		{
			get { return GetState(name); }
		}

	}
}

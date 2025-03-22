using UnityEngine;
using UnityEngine.Playables;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public partial class SimpleAnimationPlayable
	{
		private class StateInfo
		{
			public void Initialize(string name, AnimationClip clip, WrapMode wrapMode)
			{
				m_StateName = name;
				m_Clip = clip;
				m_WrapMode = wrapMode;
			}

			public float GetTime()
			{
				if (m_TimeIsUpToDate)
					return m_Time;

				m_Time = (float)m_Playable.GetTime();
				m_TimeIsUpToDate = true;
				return m_Time;
			}

			public void SetTime(float newTime)
			{
				m_Time = newTime;
				m_Playable.SetTime(m_Time);
				m_Playable.SetDone(m_Time >= m_Playable.GetDuration());
			}

			public void Enable()
			{
				if (m_Enabled)
					return;

				m_EnabledDirty = true;
				m_Enabled = true;
			}

			public void Disable()
			{
				if (m_Enabled == false)
					return;

				m_EnabledDirty = true;
				m_Enabled = false;
			}

			public void Pause()
			{
				m_Playable.Pause();
			}

			public void Play()
			{
				m_Playable.Play();
			}

			public void Stop()
			{
				m_FadeSpeed = 0f;
				ForceWeight(0.0f);
				Disable();
				SetTime(0.0f);
				m_Playable.SetDone(false);
				if (isClone)
				{
					m_ReadyForCleanup = true;
				}
			}

			public void ForceWeight(float weight)
			{
				m_TargetWeight = weight;
				m_Fading = false;
				m_FadeSpeed = 0f;
				SetWeight(weight);
			}

			public void SetWeight(float weight)
			{
				m_Weight = weight;
				m_WeightDirty = true;
			}

			public void FadeTo(float weight, float speed)
			{
				m_Fading = Mathf.Abs(speed) > 0f;
				m_FadeSpeed = speed;
				m_TargetWeight = weight;
			}

			public void DestroyPlayable()
			{
				if (m_Playable.IsValid())
				{
					m_Playable.GetGraph().DestroySubgraph(m_Playable);
				}
			}

			public void SetAsCloneOf(StateHandle handle)
			{
				m_ParentState = handle;
				m_IsClone = true;
			}

			public bool enabled
			{
				get { return m_Enabled; }
			}

			private bool m_Enabled;

			public int index
			{
				get { return m_Index; }
				set
				{
					Debug.Assert(m_Index == 0, "Should never reassign Index");
					m_Index = value;
				}
			}

			private int m_Index;

			public string stateName
			{
				get { return m_StateName; }
				set { m_StateName = value; }
			}

			private string m_StateName;

			public bool fading
			{
				get { return m_Fading; }
			}

			private bool m_Fading;


			private float m_Time;

			public float targetWeight
			{
				get { return m_TargetWeight; }
			}

			private float m_TargetWeight;

			public float weight
			{
				get { return m_Weight; }
			}

			float m_Weight;

			public float fadeSpeed
			{
				get { return m_FadeSpeed; }
			}

			float m_FadeSpeed;

			public float speed
			{
				get { return (float)m_Playable.GetSpeed(); }
				set { m_Playable.SetSpeed(value); }
			}

			public float playableDuration
			{
				get { return (float)m_Playable.GetDuration(); }
			}

			public AnimationClip clip
			{
				get { return m_Clip; }
			}

			private AnimationClip m_Clip;

			public void SetPlayable(Playable playable)
			{
				m_Playable = playable;
			}

			public bool isDone
			{
				get { return m_Playable.IsDone(); }
			}

			public Playable playable
			{
				get { return m_Playable; }
			}

			private Playable m_Playable;

			public WrapMode wrapMode
			{
				get { return m_WrapMode; }
			}

			private WrapMode m_WrapMode;

			//Clone information
			public bool isClone
			{
				get { return m_IsClone; }
			}

			private bool m_IsClone;

			public bool isReadyForCleanup
			{
				get { return m_ReadyForCleanup; }
			}

			private bool m_ReadyForCleanup;

			public StateHandle parentState
			{
				get { return m_ParentState; }
			}

			public StateHandle m_ParentState;

			public bool enabledDirty
			{
				get { return m_EnabledDirty; }
			}

			public bool weightDirty
			{
				get { return m_WeightDirty; }
			}

			public void ResetDirtyFlags()
			{
				m_EnabledDirty = false;
				m_WeightDirty = false;
			}

			private bool m_WeightDirty;
			private bool m_EnabledDirty;

			public void InvalidateTime()
			{
				m_TimeIsUpToDate = false;
			}

			private bool m_TimeIsUpToDate;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using System;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public partial class SimpleAnimationPlayable : PlayableBehaviour
	{
		private int m_StatesVersion = 0;

		private void InvalidateStates() { m_StatesVersion++; }

		private StateHandle StateInfoToHandle(StateInfo info)
		{
			return new StateHandle(this, info.index, info.playable);
		}

		private class StateManagement
		{
			private List<StateInfo> m_States;

			public int Count { get { return m_Count; } }

			private int m_Count;

			public StateInfo this[int i]
			{
				get
				{
					return m_States[i];
				}
			}

			public StateManagement()
			{
				m_States = new List<StateInfo>();
			}

			public StateInfo InsertState()
			{
				StateInfo state = new StateInfo();

				int firstAvailable = m_States.FindIndex(s => s == null);
				if (firstAvailable == -1)
				{
					firstAvailable = m_States.Count;
					m_States.Add(state);
				}
				else
				{
					m_States.Insert(firstAvailable, state);
				}

				state.index = firstAvailable;
				m_Count++;
				return state;
			}
			public bool AnyStatePlaying()
			{
				return m_States.FindIndex(s => s != null && s.enabled) != -1;
			}

			public void RemoveState(int index)
			{
				StateInfo removed = m_States[index];
				m_States[index] = null;
				removed.DestroyPlayable();
				m_Count = m_States.Count;
			}

			public bool RemoveClip(AnimationClip clip)
			{
				bool removed = false;
				for (int i = 0; i < m_States.Count; i++)
				{
					StateInfo state = m_States[i];
					if (state != null && state.clip == clip)
					{
						RemoveState(i);
						removed = true;
					}
				}
				return removed;
			}

			public StateInfo FindState(string name)
			{
				int index = m_States.FindIndex(s => s != null && s.stateName == name);
				if (index == -1)
					return null;

				return m_States[index];
			}

			public void EnableState(int index)
			{
				StateInfo state = m_States[index];
				state.Enable();
			}

			public void DisableState(int index)
			{
				StateInfo state = m_States[index];
				state.Disable();
			}

			public void SetInputWeight(int index, float weight)
			{
				StateInfo state = m_States[index];
				state.SetWeight(weight);

			}

			public void SetStateTime(int index, float time)
			{
				StateInfo state = m_States[index];
				state.SetTime(time);
			}

			public float GetStateTime(int index)
			{
				StateInfo state = m_States[index];
				return state.GetTime();
			}

			public bool IsCloneOf(int potentialCloneIndex, int originalIndex)
			{
				StateInfo potentialClone = m_States[potentialCloneIndex];
				return potentialClone.isClone && potentialClone.parentState.index == originalIndex;
			}

			public float GetStateSpeed(int index)
			{
				return m_States[index].speed;
			}
			public void SetStateSpeed(int index, float value)
			{
				m_States[index].speed = value;
			}

			public float GetInputWeight(int index)
			{
				return m_States[index].weight;
			}

			public float GetStateLength(int index)
			{
				AnimationClip clip = m_States[index].clip;
				if (clip == null)
					return 0f;
				float speed = m_States[index].speed;
				if (speed == 0f)
					return Mathf.Infinity;

				return clip.length / speed;
			}

			public float GetClipLength(int index)
			{
				AnimationClip clip = m_States[index].clip;
				if (clip == null)
					return 0f;

				return clip.length;
			}

			public float GetStatePlayableDuration(int index)
			{
				return m_States[index].playableDuration;
			}

			public AnimationClip GetStateClip(int index)
			{
				return m_States[index].clip;
			}

			public WrapMode GetStateWrapMode(int index)
			{
				return m_States[index].wrapMode;
			}

			public string GetStateName(int index)
			{
				return m_States[index].stateName;
			}

			public void SetStateName(int index, string name)
			{
				m_States[index].stateName = name;
			}

			public void StopState(int index, bool cleanup)
			{
				if (cleanup)
				{
					RemoveState(index);
				}
				else
				{
					m_States[index].Stop();
				}
			}

		}

		private struct QueuedState
		{
			public QueuedState(StateHandle s, float t)
			{
				state = s;
				fadeTime = t;
			}

			public StateHandle state;
			public float fadeTime;
		}

	}
}

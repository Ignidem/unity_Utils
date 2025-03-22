using UnityEngine;
using UnityEngine.Playables;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public partial class SimpleAnimationPlayable
	{
		public class StateHandle : IStateHandle
		{
			public StateHandle(SimpleAnimationPlayable s, int index, Playable target)
			{
				m_Parent = s;
				m_Index = index;
				m_Target = target;
			}

			public bool IsStateValid()
			{
				return m_Parent.ValidateInput(m_Index, m_Target);
			}

			public bool Enabled
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States[m_Index].enabled;
				}

				set
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					if (value)
						m_Parent.m_States.EnableState(m_Index);
					else
						m_Parent.m_States.DisableState(m_Index);
				}
			}

			public float Time
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States.GetStateTime(m_Index);
				}
				set
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					m_Parent.m_States.SetStateTime(m_Index, value);
				}
			}

			public float NormalizedTime
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");

					float length = m_Parent.m_States.GetClipLength(m_Index);
					if (length == 0f)
						length = 1f;

					return m_Parent.m_States.GetStateTime(m_Index) / length;
				}
				set
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");

					float length = m_Parent.m_States.GetClipLength(m_Index);
					if (length == 0f)
						length = 1f;

					m_Parent.m_States.SetStateTime(m_Index, value *= length);
				}
			}

			public float Speed
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States.GetStateSpeed(m_Index);
				}
				set
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					m_Parent.m_States.SetStateSpeed(m_Index, value);
				}
			}

			public string Name
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States.GetStateName(m_Index);
				}
				set
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					if (value == null)
						throw new System.ArgumentNullException("A null string is not a valid name");
					m_Parent.m_States.SetStateName(m_Index, value);
				}
			}

			public float Weight
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States[m_Index].weight;
				}
				set
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					if (value < 0)
						throw new System.ArgumentException("Weights cannot be negative");

					m_Parent.m_States.SetInputWeight(m_Index, value);
				}
			}

			public float Length
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States.GetStateLength(m_Index);
				}
			}

			public AnimationClip Clip
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States.GetStateClip(m_Index);
				}
			}

			public WrapMode WrapMode
			{
				get
				{
					if (!IsStateValid())
						throw new System.InvalidOperationException("This StateHandle is not valid");
					return m_Parent.m_States.GetStateWrapMode(m_Index);
				}
			}

			public int index
			{
				get { return m_Index; }
			}

			private SimpleAnimationPlayable m_Parent;
			private int m_Index;
			private Playable m_Target;
		}
	}
}
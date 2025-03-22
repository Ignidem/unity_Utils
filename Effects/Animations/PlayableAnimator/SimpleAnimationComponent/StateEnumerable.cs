using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtils.Effects.Animations.PlayableAnimator
{
	public partial class SimpleAnimationPlayable
	{
		private class StateEnumerable : IEnumerable<IStateHandle>
		{
			private SimpleAnimationPlayable m_Owner;

			public StateEnumerable(SimpleAnimationPlayable owner)
			{
				m_Owner = owner;
			}

			public IEnumerator<IStateHandle> GetEnumerator()
			{
				return new StateEnumerator(m_Owner);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new StateEnumerator(m_Owner);
			}

			class StateEnumerator : IEnumerator<IStateHandle>
			{
				private int m_Index = -1;
				private int m_Version;
				private SimpleAnimationPlayable m_Owner;

				public StateEnumerator(SimpleAnimationPlayable owner)
				{
					m_Owner = owner;
					m_Version = m_Owner.m_StatesVersion;
					Reset();
				}

				private bool IsValid()
				{
					return m_Owner != null && m_Version == m_Owner.m_StatesVersion;
				}

				IStateHandle GetCurrentHandle(int index)
				{
					if (!IsValid())
						throw new InvalidOperationException(
							"The collection has been modified, this Enumerator is invalid");

					if (index < 0 || index >= m_Owner.m_States.Count)
						throw new InvalidOperationException("Enumerator is invalid");

					StateInfo state = m_Owner.m_States[index];
					if (state == null)
						throw new InvalidOperationException("Enumerator is invalid");

					return new StateHandle(m_Owner, state.index, state.playable);
				}

				object IEnumerator.Current
				{
					get { return GetCurrentHandle(m_Index); }
				}

				IStateHandle IEnumerator<IStateHandle>.Current
				{
					get { return GetCurrentHandle(m_Index); }
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (!IsValid())
						throw new InvalidOperationException(
							"The collection has been modified, this Enumerator is invalid");

					do
					{
						m_Index++;
					} while (m_Index < m_Owner.m_States.Count && m_Owner.m_States[m_Index] == null);

					return m_Index < m_Owner.m_States.Count;
				}

				public void Reset()
				{
					if (!IsValid())
						throw new InvalidOperationException(
							"The collection has been modified, this Enumerator is invalid");
					m_Index = -1;
				}
			}
		}
	}
}
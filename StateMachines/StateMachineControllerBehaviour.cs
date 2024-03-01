using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.PropertyAttributes;
using Utils.StateMachines;

namespace UnityUtils.Systems.States
{
	public abstract class StateMachineControllerBehaviour<T> : MonoBehaviour, IStateMachine<Type>
		where T : IState<Type>
	{
		[SerializeReference, Polymorphic(true)]
		protected T activeState;

		[SerializeReference, Polymorphic]
		protected T[] states;

		protected IStateMachine<Type> stateMachine;
		public IState<Type> ActiveState => stateMachine?.ActiveState;
		IState IStateMachine.ActiveState => stateMachine?.ActiveState;

		public event StateChangeDelegate<Type> OnStateChange
		{
			add => stateMachine.OnStateChange += value;
			remove => stateMachine.OnStateChange -= value;
		}

		public event ExceptionHandlerDelegate OnException
		{
			add => stateMachine.OnException += value;
			remove => stateMachine.OnException -= value;
		}

		protected virtual void Awake()
		{
			try
			{
				stateMachine = new StateMachine<Type>(states.Cast<IState<Type>>());
				OnStateChange += (_, state) => activeState = (T)state;
				OnException += e => Debug.LogException(e);
			}
			catch (Exception e)
			{
				Debug.LogError($"Error in {GetType().Name} state machine");
				Debug.LogException(e);
			}
		}

		protected virtual void Start()
		{
			Type key = activeState?.GetType();
			if (key != null) SwitchState(key);
		}

		private void OnDestroy()
		{
			activeState?.Exit();
		}

		public Task SwitchState(IStateData<Type> data) => stateMachine.SwitchState(data);

		public Task SwitchState(Type key) => stateMachine.SwitchState(key);

		public Task ExitActiveState() => stateMachine.ExitActiveState();
	}
}

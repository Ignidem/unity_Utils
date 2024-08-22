using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Logger;
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
		public IStateMachine<Type>.SwitchInfo ActiveSwitch => stateMachine.ActiveSwitch;
		public bool IsSwitching => stateMachine.IsSwitching;

		public event StateChangeDelegate<Type> OnStateChange;
		public event ExceptionHandlerDelegate OnException;

		protected virtual void Awake()
		{
			InitStateMachine();
		}

		private void OnDestroy()
		{
			stateMachine.ExitActiveState().LogException();
			stateMachine.OnStateChange -= OnStateMachineStateChange;
			stateMachine.OnException -= OnStateMachineException;
		}

		protected virtual void Start()
		{
			Type key = activeState?.GetType();
			if (key != null) SwitchState(key);
		}

		public void InitStateMachine()
		{
			if (stateMachine != null)
				return;

			try
			{
				stateMachine = CreateStateMachine(states);
				stateMachine.OnStateChange += OnStateMachineStateChange;
				stateMachine.OnException += OnStateMachineException;
			}
			catch (Exception e)
			{
				Debug.LogError($"Error in {GetType().Name} state machine");
				Debug.LogException(e);
			}
		}

		protected virtual void OnStateMachineException(Exception exception)
		{
			exception.LogException();
			OnException?.Invoke(exception);
		}

		protected virtual void OnStateMachineStateChange(IState<Type> current, IState<Type> next)
		{
			activeState = (T)next;
			OnStateChange?.Invoke(current, next);
		}

		protected virtual IStateMachine<Type> CreateStateMachine(T[] states)
		{
			return new StateMachine<Type>(states.Cast<IState<Type>>());
		}

		public Task SwitchState(IStateData<Type> data) => stateMachine.SwitchState(data);

		public Task SwitchState(Type key) => stateMachine.SwitchState(key);

		public Task ExitActiveState() => stateMachine.ExitActiveState();
	}
}

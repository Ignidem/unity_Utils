using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Logger;
using UnityUtils.PropertyAttributes;
using Utils.StateMachines;
using System.Runtime.CompilerServices;
using Utils.Results;

namespace UnityUtils.Systems.States
{
	public abstract class StateMachineControllerBehaviour<T> : MonoBehaviour, IStateMachine<Type>, IDisposable
		where T : IState<Type>
	{
		[SerializeReference, Polymorphic(PolymorphicSettings.Nullable | PolymorphicSettings.IgnoreChildren)]
		protected T activeState;

		[SerializeReference, Polymorphic]
		protected T[] states;

		protected IStateMachine<Type> stateMachine;
		public IState<Type> ActiveState => stateMachine?.ActiveState;
		public IState<Type> NextState => stateMachine?.NextState;
		public bool IsTransitioning => stateMachine.IsTransitioning;

		public event StateChangeDelegate<Type> OnStateChange;
		public event TransitionDelegate OnTransition;
		public event ExceptionHandlerDelegate OnException;

		protected virtual void Awake()
		{
			InitStateMachine();
		}

		private void OnDestroy()
		{
			Dispose();
		}

		public virtual void Dispose()
		{
			stateMachine.ExitActiveState().LogException();
			stateMachine.OnStateChange -= OnStateMachineStateChange;
			stateMachine.OnTransition -= OnTransitionChange;
			stateMachine.OnException -= OnStateMachineException;
		}

		protected virtual void Start()
		{
			Type key = activeState?.GetType();
			if (key != null) SwitchState(key);
		}

		public Type GetKeyAt(int index)
		{
			return states[index].Key;
		}

		public void InitStateMachine()
		{
			if (stateMachine != null)
				return;

			try
			{
				stateMachine = CreateStateMachine(states);
				stateMachine.OnStateChange += OnStateMachineStateChange;
				stateMachine.OnTransition += OnTransitionChange;
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
		private void OnTransitionChange(TransitionType type)
		{
			OnTransition?.Invoke(type);
		}

		protected virtual IStateMachine<Type> CreateStateMachine(T[] states)
		{
			return new StateMachine<Type>(states.Cast<IState<Type>>());
		}

		public Task<Result> SwitchState(IState<Type> state) => stateMachine.SwitchState(state);
		public Task<Result> SwitchState(IStateData<Type> data) => stateMachine.SwitchState(data);
		public Task<Result> SwitchState(Type key) => stateMachine.SwitchState(key);
		public Task ExitActiveState() => stateMachine.ExitActiveState();
		public TaskAwaiter GetAwaiter() => stateMachine.GetAwaiter();
		public bool ContainsState(Type key) => stateMachine.ContainsState(key);
		public void AddOrReplaceState(IState<Type> state) => stateMachine.AddOrReplaceState(state);
	}
}

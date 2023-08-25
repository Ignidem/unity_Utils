using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils.PropertyAttributes;
using Utils.StateMachines;

namespace Assets.Systems.States
{
	public abstract class StateMachineControllerBehaviour<T> : MonoBehaviour
	{
		[SerializeReference, Polymorphic]
		protected T activeState;

		[SerializeReference, Polymorphic]
		protected T[] states;

		protected IStateMachine<Type> stateMachine;

		protected virtual void Awake()
		{
			stateMachine = new StateMachine<Type>(states.Cast<IState<Type>>());
			stateMachine.OnStateChange += (_, state) => activeState = (T)state;
		}

		protected virtual void Start() 
		{
			stateMachine.SwitchState(activeState.GetType());
		}
	}
}

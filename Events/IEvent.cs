using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.AddressableUtils;

namespace UnityUtils.Events
{
	public interface IEvent
	{
		void Invoke(object sender);
	}

	public interface IEvent<TSource> : IEvent
	{
		void Invoke(TSource sender);
	}

	public interface IAsyncEvent<TSource> : IEvent<TSource>
	{
		Task InvokeAsync(TSource sender);
	}

	//WIP
	public struct InstantiateEvent<TSource, T> : IAsyncEvent<TSource>
		where TSource : Object
		where T : Object
	{
		[SerializeField]
		private AddressableReference<T> target;

		[Tooltip("Should the object be instantiated in the source. Otherwise will be at source's position")]
		private bool inSource;

		public void Invoke(object sender)
		{
			throw new System.NotImplementedException();
		}

		public void Invoke(TSource sender)
		{
			throw new System.NotImplementedException();
		}

		public Task InvokeAsync(TSource sender)
		{
			throw new System.NotImplementedException();
		}

		private async Task Instantiate(Transform parent)
		{
			throw new System.NotImplementedException();
		}
	}
}

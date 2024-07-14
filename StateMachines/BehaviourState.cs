using System.Threading.Tasks;
using UnityEngine;
using UnityUtils.AddressableUtils;
using Utils.StateMachines;

namespace UnityUtils.Systems.States
{
	public abstract class BehaviourState<TComponent, TKey> : State<TKey>
		where TComponent : MonoBehaviour
	{
		[SerializeField] private LazyAddressable<TComponent> viewAddressable;

		protected TComponent Instance { get; private set; }
		protected virtual bool DoReleaseAsset => true;

		protected override async Task OnPreload(IStateData<TKey> data)
		{
			if (!Instance)
			{
				Instance = await viewAddressable;
				Instance.transform.localPosition = Vector3.zero;
				if (Instance.transform is RectTransform rect)
					rect.sizeDelta = Vector3.zero;
			}
		}

		protected override Task OnExit()
		{
			/*
			if (Instance)
				Instance.gameObject.SetActive(false);
			*/
			return base.OnExit();
		}

		protected override Task OnCleanup()
		{
			if (DoReleaseAsset)
			{
				viewAddressable.Dispose();
			}

			return base.OnCleanup();
		}
	}
}

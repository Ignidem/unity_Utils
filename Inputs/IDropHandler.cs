using System.Threading.Tasks;

namespace UnityUtils.Inputs
{
	public interface IDropHandler<T>
	{
		bool OnDrop(T target);
	}

	public interface IDropHandlerAsync<T>
	{
		Task<bool> OnDrop(T target);
	}
}

#if ENABLE_INPUT_SYSTEM
namespace UnityUtils.CSharpInputListener
{
	public interface IInputReceiver
	{
		bool IsActive { get; }
	}
}
#endif
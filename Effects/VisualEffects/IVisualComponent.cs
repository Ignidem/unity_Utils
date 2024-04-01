using System;
using System.Threading.Tasks;

namespace UnityUtils.Effects.VisualEffects
{
	public interface IVisualComponent : IDisposable
	{
		void Play();
		void Stop();
	}
}

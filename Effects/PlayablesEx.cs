using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityUtils.Effects
{
	public static class PlayablesEx
	{
		public static T ConnectBehaviour<T>(this PlayableGraph graph, int index)
			where T : class, IPlayableBehaviour, new()
		{
			return graph.ConnectBehaviour<Playable, T>(graph.GetRootPlayable(index));
		}

		public static T ConnectBehaviour<U, T>(this PlayableGraph graph, U node)
			where T : class, IPlayableBehaviour, new()
			where U : struct, IPlayable
		{
			ScriptPlayable<T> behaviour = ScriptPlayable<T>.Create(graph, 1);
			graph.Connect(node, 0, behaviour, 0);
			return behaviour.GetBehaviour();
		}

		public static IEnumerable<Playable> GetInputs(this Playable playable)
		{
			int count = playable.GetInputCount();
			for (int i = 0; i < count; i++)
				yield return playable.GetInput(i);
		}

		public static IEnumerable<Playable> GetInputs<T>(this Playable playable, bool recursive)
			where T : struct, IPlayable
		{
			int count = playable.GetInputCount();
			for (int i = 0; i < count; i++)
			{
				var p = playable.GetInput(i);
				System.Type type = p.GetPlayableType();
				if (type == typeof(T))
					yield return p;

				if (!recursive)
					continue;

				foreach (var p2 in p.GetInputs<T>(true))
					yield return p2;
			}
		}
	}
}

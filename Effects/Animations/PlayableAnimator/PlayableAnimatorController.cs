using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityUtils.Animations.PlayableAnimator
{
	[Serializable]
	public struct OutputDefinition
	{
		public string name;
		public LayerDefinition[] layers;
	}

	[Serializable]
	public struct LayerDefinition
	{
		public int index;
		public AnimationClip[] animations;
	}

	public class PlayableAnimatorController
	{
		private readonly PlayableGraph playableGraph;

		public PlayableAnimatorController(Animator animator, params OutputDefinition[] outputs)
		{
			playableGraph = PlayableGraph.Create();
			playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

			for (int o = 0; o < outputs.Length; o++)
			{
				OutputDefinition outputDef = outputs[o];
				AnimationPlayableOutput output = AnimationPlayableOutput.Create(playableGraph, outputDef.name, animator);
				AnimationLayerMixerPlayable layers = AnimationLayerMixerPlayable.Create(playableGraph, outputDef.layers.Length);
				output.SetSourcePlayable(layers);

				for (int l = 0; l < outputDef.layers.Length; l++)
				{
					LayerDefinition layer = outputDef.layers[l];
					var mixer = AnimationMixerPlayable.Create(playableGraph);
					playableGraph.Connect(mixer, 0, layers, l);

					for (int i = 0; i < layer.animations.Length; i++)
					{
						AnimationClip clip = layer.animations[i];
						if (!clip) continue;
						AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
						playableGraph.Connect(clipPlayable, 0, mixer, i);
					}
				}
			}
		}
	}
}

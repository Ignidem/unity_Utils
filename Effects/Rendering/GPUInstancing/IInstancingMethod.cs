namespace UnityUtils.Effects.Rendering.GPUInstancing
{
	public interface IInstancingMethod
	{
		void Validate();
		void Setup();
		void Draw();
	}
}

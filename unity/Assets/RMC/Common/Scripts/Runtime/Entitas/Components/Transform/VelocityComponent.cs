using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Transform
{
	/// <summary>
	/// Stores entities current velocity
	/// </summary>
	public class VelocityComponent : IComponent
	{
		// ------------------ Serialized fields and properties
		public Vector3 velocity;

	}
}
using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Transform
{
	/// <summary>
	/// Stores Entity's friction to be applied against the velocity
	/// </summary>
	public class FrictionComponent : IComponent
	{
		// ------------------ Serialized fields and properties
        public Vector3 friction;

	}
}
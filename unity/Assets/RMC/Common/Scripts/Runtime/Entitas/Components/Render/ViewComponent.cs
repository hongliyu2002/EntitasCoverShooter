using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Render
{
	/// <summary>
	/// Stores the gameObject and similar info to render the Entity 
	/// </summary>
	public class ViewComponent : IComponent
	{
		// ------------------ Serialized fields and properties
		public object gameObject;
        public Bounds bounds;

	}
}
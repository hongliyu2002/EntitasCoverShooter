using Entitas;
using Entitas.CodeGenerator;

namespace RMC.Common.Entitas.Components.Audio
{
	/// <summary>
	/// Control high-level audio configuration
	/// </summary>
    [SingleEntity]
	public class AudioSettingsComponent : IComponent
	{
		// ------------------ Serialized fields and properties
        public bool isMuted;

	}
}
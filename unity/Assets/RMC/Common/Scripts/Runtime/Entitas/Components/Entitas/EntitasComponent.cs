using Entitas;
using Entitas.CodeGenerator;
using EntitasSystems = Entitas.Systems;

namespace RMC.Common.Entitas.Components.Entitas
{
    /// <summary>
    /// This is custom and optional. I use it to store the systems in case I need them again. 
    /// I'm not sure this is useful, but I saw something similar in Entitas presentation slides - srivello
    /// </summary>
    [SingleEntity]
    public class EntitasComponent : IComponent
    {
        // ------------------ Serialized fields and properties
        public EntitasSystems pausableUpdateSystems;
        public EntitasSystems unpausableUpdateSystems;
        public EntitasSystems pausableFixedUpdateSystems;
    }
}

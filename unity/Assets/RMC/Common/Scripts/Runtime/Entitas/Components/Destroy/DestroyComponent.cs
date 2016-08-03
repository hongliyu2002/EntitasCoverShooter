using Entitas;
using Entitas.CodeGenerator;

namespace RMC.Common.Entitas.Components.Destroy
{
    /// <summary>
    /// Sent like an event: Will destroy the Entity
    /// </summary>
    public class DestroyMeComponent : IComponent
    {
        public float delayBeforeDestroy;
    }
}

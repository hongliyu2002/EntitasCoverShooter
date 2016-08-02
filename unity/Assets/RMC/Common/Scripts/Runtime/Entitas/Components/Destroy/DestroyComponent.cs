using Entitas;
using Entitas.CodeGenerator;

namespace RMC.Common.Entitas.Components.Destroy
{
    /// I prefer entity.WillDestroy(true) instead of entity.WillDestroy(True). Done! :)
    [CustomPrefix("Will")]

    /// <summary>
    /// Sent like an event: Will destroy the Entity
    /// </summary>
    public class DestroyComponent : IComponent
    {
    }
}

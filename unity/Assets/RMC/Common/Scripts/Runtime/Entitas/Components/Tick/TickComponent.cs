using Entitas;

namespace RMC.Common.Entitas.Components.Tick
{
    /// <summary>
    /// Every 'tick' (typically each monoBehavior.Update() or FixedUpdate() Time.deltaTime is stored
    /// </summary>
    public class TickComponent : IComponent
    {
        // ------------------ Serialized fields and properties
        public float deltaTime = 1;
    }
}
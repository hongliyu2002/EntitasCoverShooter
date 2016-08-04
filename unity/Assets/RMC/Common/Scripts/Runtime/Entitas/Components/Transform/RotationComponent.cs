using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Transform
{
    /// <summary>
    /// Stores entity's current rotation
    /// </summary>
    public class Rotation : IComponent
    {
        // ------------------ Serialized fields and properties
        public Vector3 rotation;
        public bool useTween;
    }
}
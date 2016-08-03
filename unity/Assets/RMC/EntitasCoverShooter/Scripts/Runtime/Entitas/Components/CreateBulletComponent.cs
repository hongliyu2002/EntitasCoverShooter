using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.EntitasCoverShooter.Entitas.Components
{
    /// <summary>
    /// Sent like an event: Shoot a bullet
    /// </summary>
    public class CreateBulletComponent : IComponent
    {
        // ------------------ Serialized fields and properties
        public Vector3 fromPosition;
        public Vector3 toPosition;
        public float speed;

    }
}
using Entitas;
using RMC.Common.UnityEngineReplacement;


namespace Entitas
{
    /// <summary>
    /// Convenience: Create Entities directly on pool
    /// This is an alternative to copy/pasting the creation code whenever needed.
    /// This is an alternative to creating a new component system for every new entity needed.
    /// </summary>
    public static class PoolExtensions
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields

        // ------------------ Methods
        public static Entity CreateEffect(this Pool pool, Vector3 position, string resourcePath )
        {
            Entity entity           = pool.CreateEntity();
            entity.AddPosition      (position, false);
            entity.AddResource      (resourcePath);

            //keep short until I can figure out how to shorten the particle lifetime 
            //(looks like looping now, but looping bool is not true)
            entity.AddDestroyMe     (1); 

            //Most effects won't rotate by rotating the game object. Must manually edit prefab
            entity.AddRotation      (RMC.Common.UnityEngineReplacement.Vector3.zero, false);
            return entity;
        }

        public static Entity CreatePlayerBullet(this Pool pool, Vector3 fromPosition, Vector3 toPosition, float speed)
        {
            return pool.CreateBullet("Prefabs/BulletPlayer", fromPosition, toPosition, speed);
        }
        public static Entity CreateEnemyBullet(this Pool pool, Vector3 fromPosition, Vector3 toPosition, float speed)
        {
            return pool.CreateBullet("Prefabs/BulletEnemy", fromPosition, toPosition, speed);
        }
        private static Entity CreateBullet(this Pool pool, string resourcePath, Vector3 fromPosition, Vector3 toPosition, float speed)
        {

            Entity entity = pool.CreateEntity ();
            entity.AddPosition (fromPosition, false);

            //
            Vector3 newVelocity         =   (toPosition - fromPosition).Normalize() * speed;
            entity.AddFriction        (RMC.Common.UnityEngineReplacement.Vector3.zero);
            entity.AddResource        (resourcePath);
            entity.AddVelocity        (newVelocity);
            entity.AddTick            (0);
            entity.AddDestroyMe       (4);
            entity.AddRotation        (RMC.Common.UnityEngineReplacement.Vector3.zero, false);

            return entity;

        }

    }
}

using Entitas;
using RMC.Common.Entitas.Components;
using System;
using System.Collections.Generic;
using RMC.EntitasCoverShooter.Entitas;
using RMC.EntitasCoverShooter.Entitas.Controllers.Singleton;
using System.Collections;
using RMC.Common.UnityEngineReplacement;
using RMC.Common.Utilities;

namespace RMC.EntitasCoverShooter.Entitas.Systems.GameState
{
    /// <summary>
    /// Called at game start and after every goal
    /// </summary>
    public class CreateBulletSystem : ISetPool, IReactiveSystem
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields
        private Pool _pool;

        // ------------------ Methods

        // Implement ISetPool to get the pool used when calling
        // pool.CreateSystem<FooSystem>();
        public void SetPool(Pool pool) 
        {
            // Get the group of entities that have these component(s)
            _pool = pool;
        }

        public TriggerOnEvent trigger
        {
            get { return Matcher.CreateBullet.OnEntityAdded(); }
        }

        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                Entity bulletEntity = _pool.CreateEntity ();
                bulletEntity.AddPosition (entity.createBullet.fromPosition);

                //
                Vector3 newVelocity         =   (entity.createBullet.toPosition - entity.createBullet.fromPosition).Normalize() * entity.createBullet.speed;
                bulletEntity.AddFriction        (RMC.Common.UnityEngineReplacement.Vector3.zero);
                bulletEntity.AddResource        ("Prefabs/Bullet");
                bulletEntity.AddVelocity        (newVelocity);
                bulletEntity.AddTick            (0);
                bulletEntity.AddDestroyMe         (4);

                //  The Entity holding the CreateBulletComponent has been processed, so destroy the related Entity
                entity.AddDestroyMe(0);

            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entitas;
using RMC.Common.Utilities;
using System.Collections;

namespace RMC.Common.Entitas.Systems.Destroy
{
    /// <summary>
    /// Destroy's the Entity
    /// </summary>
    public class DestroySystem : ISetPool, IReactiveSystem 
    {
        private Pool _pool;

        public TriggerOnEvent trigger
        {
            get { return Matcher.DestroyMe.OnEntityAdded(); }
        }

        public void SetPool(Pool pool)
        {
            _pool = pool;
        }


        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.destroyMe.delayBeforeDestroy == 0)
                {
                    //UnityEngine.Debug.Log("here " + entity);
                    DestroyEntity(entity);
                }
                else
                {
                    //Must make a local reference to the entityToDestroy
                    //Otherwise we lose state and the destroy breaks
                    Entity entityToDestroy = entity;
                    Timer.Register(entity.destroyMe.delayBeforeDestroy, () => DestroyEntity(entityToDestroy));
                }
            }
               
        }

        //Its possible that we create a bullet and say 'destroy yourself in 4 seconds' and then the bullet collides and we say destroy yourself now,
        //then the 4 seconds expires and it tries to delete itself again.
        //So this 'if' solves that - srivello
        private void DestroyEntity (Entity entity)
        {
            if (_pool.HasEntity(entity))
            {
                _pool.DestroyEntity(entity);
            }
            else
            {
                entity.destroy();
            }
        }


    }
}

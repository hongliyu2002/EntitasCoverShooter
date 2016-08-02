using Entitas;
using RMC.Common.Entitas.Components;
using System;
using System.Collections.Generic;
using RMC.EntitasCoverShooter.Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Systems
{
    /// <summary>
    /// Constains the balls's y position within the screenbounds with a bounce.
    /// Great example of a system that operates ONLY when a component (position) is changed. Efficient!
    /// </summary>
    public class BoundsBounceSystem : ISystem, ISetPool
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields
        private Group _group;
        private Entity _gameEntity;
        private Bounds _bounds;
        private Pool _pool;

        // ------------------ Methods

        // Implement ISetPool to get the pool used when calling
        // pool.CreateSystem<MoveSystem>();
        public void SetPool(Pool pool) 
        {
            _pool = pool;
            _group = _pool.GetGroup(Matcher.AllOf(Matcher.BoundsBounce, Matcher.Velocity, Matcher.Position, Matcher.View));
            _group.OnEntityUpdated += Group_OnEntityUpdated;

            //By design: Systems created before Entities, so wait :)
            _pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds)).OnEntityAdded += GameGroup_OnEntityAdded;

        }

        private void GameGroup_OnEntityAdded (Group group, Entity entity, int index, IComponent component)
        {
            _gameEntity = group.GetSingleEntity();
            _bounds = _gameEntity.bounds.bounds;
        }

        //I explored several ways to have this system only respond when a paddle's position changes
        //1. _group.OnEntityUpdated += OnPaddlePositionUpdated; I'm using this now.
        //2. I couldn't find a way to do it with "public TriggerOnEvent trigger". its more about entity add than components, yes?
        //3. _onPaddlePositionUpdated = _group.CreateObserver(GroupEventType.OnEntityAdded). its more about entity add than components, yes?
        private void Group_OnEntityUpdated (Group group, Entity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            float sizeY = entity.view.bounds.size.y / 2;
            Vector3 nextVelocity = entity.velocity.velocity;
            Vector3 nextPosition = entity.position.position;
            float bounceAmount = entity.boundsBounce.bounceAmount;

            //Bottom
            if (entity.position.position.y - sizeY < _bounds.min.y) 
            {
                nextVelocity = new Vector3 (nextVelocity.x, nextVelocity.y * bounceAmount, nextVelocity.z);
                _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_Collision, GameConstants.AudioVolume);

                //order matters
                //1
                entity.ReplacePosition(new Vector3 (nextPosition.x, _bounds.min.y + sizeY, nextPosition.z));
                //2
                entity.ReplaceVelocity(nextVelocity);
            } 
            //Top
            else if (entity.position.position.y + sizeY > _bounds.max.y)
            {
                nextVelocity = new Vector3 (nextVelocity.x, nextVelocity.y * bounceAmount, nextVelocity.z);
                _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_Collision, GameConstants.AudioVolume);

                //order matters
                //1
                entity.ReplacePosition(new Vector3 (nextPosition.x, _bounds.max.y - sizeY, nextPosition.z));
                //2
                entity.ReplaceVelocity(nextVelocity);
            }


        }

    }
}



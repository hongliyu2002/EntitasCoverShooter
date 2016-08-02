using System.Collections.Generic;
using Entitas;
using RMC.Common.Singleton;
using UnityEngine;
using RMC.Common.Entitas.Utilities;

namespace RMC.Common.Entitas.Systems.Render
{

    /// <summary>
    /// Updates with Time.deltaTime
    /// </summary>
    public class TickController : SingletonMonobehavior<TickController>
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields
        private Group _tickGroup;

        // ------------------ Methods

        protected void Start()
        {
            _tickGroup = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Tick));
            _tickGroup.OnEntityAdded += PositionGroup_OnEntityAdded;

            // Start() may be called AFTER Entities exist. So process latent Entities now.
            ProcessTickEntities(_tickGroup.GetEntities());

        }

        override protected void OnDestroy()
        {
            base.OnDestroy();
            _tickGroup.OnEntityAdded -= PositionGroup_OnEntityAdded;
        }

        private void PositionGroup_OnEntityAdded (Group group, Entity entity, int index, IComponent component)
        {
            ProcessTickEntity(entity);
        }

        private void ProcessTickEntities (Entity[] entities)
        {
            foreach (Entity entity in entities)
            {
                ProcessTickEntity(entity);
            }
        }

        /// <summary>
        /// ENTITAS_HELP_REQUEST: What is the best way to call ReplaceX() 
        /// without recursively calling XGroup.OnEntityAdded?
        /// I'd like to remove the if below. -srivello
        /// </summary>
        private void ProcessTickEntity (Entity entity)
        {
            //TODO: Remove if
            if (entity.tick.deltaTime != Time.deltaTime)
            {
                entity.ReplaceTick(Time.deltaTime);
            }
        }

    }
}


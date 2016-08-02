using System.Collections.Generic;
using Entitas;
using RMC.Common.Singleton;
using UnityEngine;
using RMC.Common.Entitas.Utilities;

namespace RMC.Common.Entitas.Systems.Render
{

    /// <summary>
    /// Updates the View to reflect the data
    /// </summary>
    public class ViewController : SingletonMonobehavior<ViewController>
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        public UnityEngine.Transform ViewsParent
        {
            get
            {
                return _viewsParent;
            }

        }

        // ------------------ Non-serialized fields
        private Group _positionGroup;
        private UnityEngine.Transform _viewsParent;

        // ------------------ Methods

        protected void Start()
        {
            _viewsParent = new GameObject("Views").transform;
            _viewsParent.parent = transform;


            _positionGroup = Pools.pool.GetGroup(Matcher.AllOf(Matcher.View, Matcher.Position));
            _positionGroup.OnEntityAdded += PositionGroup_OnEntityAdded;

            // Start() may be called AFTER Entities exist. So process latent Entities now.
            ProcessPositionEntities(_positionGroup.GetEntities());

        }

        override protected void OnDestroy()
        {
            base.OnDestroy();
            _positionGroup.OnEntityAdded -= PositionGroup_OnEntityAdded;
        }

        private void PositionGroup_OnEntityAdded (Group group, Entity entity, int index, IComponent component)
        {
            ProcessPositionEntity(entity);
        }

        private void ProcessPositionEntities (Entity[] entities)
        {
            foreach (Entity entity in entities)
            {
                ProcessPositionEntity(entity);
            }
        }

        private void ProcessPositionEntity (Entity entity)
        {
            
            ((GameObject)entity.view.gameObject).transform.position = UnityEngineReplacementUtility.Convert(entity.position.position);
            //Debug.Log("ProcessEntity: " + ((GameObject)entity.view.gameObject).transform.position);
        }

    }
}


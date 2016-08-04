using System.Collections.Generic;
using Entitas;
using RMC.Common.Singleton;
using UnityEngine;
using RMC.Common.Entitas.Utilities;
using DG.Tweening;

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
        private bool _initialized = false;
        private Group _positionGroup;
        private UnityEngine.Transform _viewsParent;

        // ------------------ Methods

        protected void Start()
        {
            Execute();

        }

        private void Initialize()
        {
            if (!_initialized)
            {
                _initialized = true;
                _viewsParent = new GameObject("Views").transform;
                _viewsParent.parent = transform;


                _positionGroup = Pools.pool.GetGroup(Matcher.AllOf(Matcher.View, Matcher.Position, Matcher.Rotation));
                _positionGroup.OnEntityAdded += PositionGroup_OnEntityAdded;
            }
        }

        public void Execute ()
        {
            Initialize();
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

            if (!entity.position.useTween)
            {
                ((GameObject)entity.view.gameObject).transform.position = UnityEngineReplacementUtility.Convert(entity.position.position);
                    
            }
            else
            {
                ((GameObject)entity.view.gameObject).transform.DOMove(UnityEngineReplacementUtility.Convert(entity.position.position), 0.3f);
            }

            if (!entity.position.useTween)
            {
                ((GameObject)entity.view.gameObject).transform.rotation = Quaternion.Euler(UnityEngineReplacementUtility.Convert(entity.rotation.rotation));

            }
            else
            {
                ((GameObject)entity.view.gameObject).transform.DORotate(UnityEngineReplacementUtility.Convert(entity.rotation.rotation), 0.3f);
            }
        
            //Debug.Log("ProcessEntity: " + ((GameObject)entity.view.gameObject).transform.position);
        }

    }
}


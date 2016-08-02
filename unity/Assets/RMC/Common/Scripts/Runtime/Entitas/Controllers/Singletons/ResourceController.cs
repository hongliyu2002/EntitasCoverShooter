using System.Collections.Generic;
using Entitas;
using RMC.Common.Singleton;
using UnityEngine;
using RMC.Common.Entitas.Utilities;
using System;

namespace RMC.Common.Entitas.Systems.Render
{

    /// <summary>
    /// Updates the View to reflect the data
    /// </summary>
    public class ResourceController : SingletonMonobehavior<ResourceController>
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        private Group _resourceGroup;


        // ------------------ Non-serialized fields

        // ------------------ Methods

        protected void Start()
        {

            _resourceGroup = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Resource));
            _resourceGroup.OnEntityAdded += ResourceGroup_OnEntityAdded;
            _resourceGroup.OnEntityRemoved += ResourceGroup_OnEntityRemoved;


            // Start() may be called AFTER Entities exist. So process latent Entities now.
            ProcessResourceAddedEntities(_resourceGroup.GetEntities());

        }

        override protected void OnDestroy()
        {
            base.OnDestroy();
            _resourceGroup.OnEntityAdded -= ResourceGroup_OnEntityAdded;
            _resourceGroup.OnEntityRemoved -= ResourceGroup_OnEntityRemoved;
        }




        private void ResourceGroup_OnEntityAdded (Group group, Entity entity, int index, IComponent component)
        {
            ProcessResourceAddedEntity(entity);
        }

        private void ProcessResourceAddedEntities (Entity[] entities)
        {
            foreach (Entity entity in entities)
            {
                ProcessResourceAddedEntity(entity);
            }
        }

        private void ProcessResourceAddedEntity (Entity entity)
        {

            var resource = Resources.Load<GameObject>(entity.resource.resourcePath);
            GameObject gameObject = null;
            try {
                gameObject = UnityEngine.Object.Instantiate(resource);

            } catch (Exception) {
                Debug.Log("Cannot instantiate " + resource);
            }

            if (gameObject != null) 
            {
                gameObject.transform.parent = ViewController.Instance.ViewsParent;

                //We want the size here. So store the bounds.
                //null is ok
                Collider collider = gameObject.GetComponent<Collider>();
                if (collider != null)
                {
                    entity.AddView(gameObject, UnityEngineReplacementUtility.Convert (collider.bounds));
                }
                else
                {
                    entity.AddView(gameObject, new RMC.Common.UnityEngineReplacement.Bounds ());
                }

                //Keep
                //Debug.Log("View Added: " + entity.view.gameObject);


            }
        }





        private void ResourceGroup_OnEntityRemoved (Group group, Entity entity, int index, IComponent component)
        {
            ProcessResourceRemovedEntity(entity);
        }

        private void ProcessResourceRemovedEntity (Entity entity)
        {
            UnityEngine.Object.Destroy((UnityEngine.Object)entity.view.gameObject);

        }
            

    }
}


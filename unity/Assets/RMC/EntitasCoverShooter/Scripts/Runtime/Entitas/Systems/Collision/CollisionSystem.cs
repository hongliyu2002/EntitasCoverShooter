using System.Collections.Generic;
using System.Linq;
using Entitas;
using RMC.Common.Entitas.Components.Collision;
using RMC.Common.UnityEngineReplacement;

namespace RMC.EntitasCoverShooter.Entitas.Systems.Collision
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
    public class CollisionSystem : ISetPool, IInitializeSystem, IReactiveSystem
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
		public TriggerOnEvent trigger 
		{ 
			get 
			{ 
				return Matcher.Collision.OnEntityAdded(); 
			} 
		}


		// ------------------ Non-serialized fields
        private Pool _pool;
		private Group _group;

		// ------------------ Methods
		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<FooSystem>();
		public void SetPool(Pool pool) 
		{
            _pool = pool;
	
		}

        public void Initialize ()
        {
            // Get the group of entities that have these component(s)
            _group = _pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Velocity));

        }

		public void Execute(List<Entity> entities) 
		{
			foreach (var collisionEntity in entities) 
			{
                //The collision may happen on the same frame as the ball is removed after a goal
                var bulletEntity = _group.GetEntities().FirstOrDefault(e2 => e2.view.gameObject == collisionEntity.collision.localGameObject);

                if (collisionEntity.collision.collisionType == CollisionComponent.CollisionType.TriggerEnter && 
                    bulletEntity != null)
				{
					//Find entities from the unity data
					
                    var enemyEntity = _group.GetEntities().FirstOrDefault(e2 => e2.view.gameObject == collisionEntity.collision.localGameObject);
                    if (enemyEntity != null )
                    {
                        //UnityEngine.Debug.Log (collisionEntity.collision.localGameObject + " with " + collisionEntity.collision.otherGameObject);

                        //  We know that ALL bullets already have a DestroyMe (X second delay) upon creation
                        //  but check with an 'if' in case we change that later.
                        if (bulletEntity.hasDestroyMe)
                        {
                            bulletEntity.ReplaceDestroyMe(0);
                        }
                        else
                        {
                            bulletEntity.AddDestroyMe(0);
                        }

                        _pool.CreateEffect(bulletEntity.position.position, "Prefabs/Effects/ExplosionEnemy");

                        //UnityEngine.Debug.Log("bulletEntity: " + bulletEntity);
                        _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_Collision, GameConstants.AudioVolume);
                    }
					
				}
                collisionEntity.AddDestroyMe(0);
	        }

	   }
	}
}


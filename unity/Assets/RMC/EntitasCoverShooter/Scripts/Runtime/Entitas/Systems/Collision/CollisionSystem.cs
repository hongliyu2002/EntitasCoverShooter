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
                var entity = _group.GetEntities().FirstOrDefault(e2 => e2.view.gameObject == collisionEntity.collision.localGameObject);

                if (collisionEntity.collision.collisionType == CollisionComponent.CollisionType.TriggerEnter && entity != null)
				{
					//Find entities from the unity data
					
					var paddleEntity = _group.GetEntities().FirstOrDefault(e2 => e2.view.gameObject == collisionEntity.collision.localGameObject);
                    if (paddleEntity != null)
                    {
                        //Debug.Log (collisionEntity.collision.collider.gameObject);
					
                        //Move the ball and include some of the paddle's y velocity to 'steer' the ball
                        Vector3 nextVelocity = entity.velocity.velocity;
                        Vector3 paddleVelocity = paddleEntity.velocity.velocity;
                        entity.ReplaceVelocity 
                        (
                            new Vector3(nextVelocity.x * GameConstants.PaddleBounceAmountX, nextVelocity.y + paddleVelocity.y * GameConstants.PaddleFrictionY, nextVelocity.z)
                        );
                        _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_Collision, GameConstants.AudioVolume);
                    }
					
				}
                collisionEntity.WillDestroy(true);
	        }

	   }
	}
}


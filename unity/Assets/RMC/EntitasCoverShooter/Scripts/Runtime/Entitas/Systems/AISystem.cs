using Entitas;
using RMC.Common.Entitas.Components;
using RMC.Common.Entitas.Components.Render;
using RMC.Common.Entitas.Components.Transform;
using RMC.Common.UnityEngineReplacement;

namespace RMC.EntitasCoverShooter.Entitas.Systems
{
    /// <summary>
    /// Stores how the computer Paddle responds to the ball
    /// </summary>
	public class AISystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _aiGroup;


		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_aiGroup = pool.GetGroup(Matcher.AllOf(Matcher.AI, Matcher.Position, Matcher.Velocity));
			
            Group ballCreatedGroup = pool.GetGroup(Matcher.AllOf(Matcher.Goal, Matcher.Position).NoneOf (Matcher.Destroy));
			ballCreatedGroup.OnEntityAdded += BallCreatedGroup_OnEntityAdded;

            Group ballDestroyGroup = pool.GetGroup(Matcher.AllOf(Matcher.Goal, Matcher.Position, Matcher.Destroy));
			ballDestroyGroup.OnEntityAdded += BallDestroyGroup_OnEntityAdded;
			
	

		}

		//Whenever a new ball is created, follow it
		protected virtual void BallCreatedGroup_OnEntityAdded(Group collection, Entity ballEntity, int index, IComponent component)
		{
			//Debug.Log ("created" + ballEntity);
            foreach (var entity in _aiGroup.GetEntities()) 
			{
				entity.ReplaceAI (ballEntity, entity.aI.deadZoneY, entity.aI.velocityY);
			}
		}

		//whenever a ball is destroyed, stop following it.
		protected virtual void BallDestroyGroup_OnEntityAdded(Group collection, Entity ballEntity, int index, IComponent component)
		{
			//Debug.Log ("destroy" + ballEntity);
            foreach (var entity in _aiGroup.GetEntities()) 
			{
				entity.ReplaceAI (null, entity.aI.deadZoneY, entity.aI.velocityY);
                entity.ReplaceVelocity ( new RMC.Common.UnityEngineReplacement.Vector3 (0,0,0));
			}
		}

		public void Execute() 
		{

            foreach (var entity in _aiGroup.GetEntities()) 
			{
				Vector3 nextVelocity = Vector3.zero;
		

                //THe ball is destroyed after each goal, so we do some checks to NOT follow it at that moment - srivello
				Entity targetEntity = entity.aI.targetEntity;
				if (targetEntity != null)
				{
                    if (targetEntity.hasPosition)
                    {

                        Vector3 targetPosition = targetEntity.position.position;
                        if (targetPosition.y > entity.position.position.y + entity.aI.deadZoneY)
                        {
                            nextVelocity = new Vector3(0, entity.aI.velocityY, 0);
                        }
                        else if (targetPosition.y < entity.position.position.y - entity.aI.deadZoneY)
                        {
                            nextVelocity = new Vector3(0, -entity.aI.velocityY, 0);
                        }

                        entity.ReplaceVelocity(nextVelocity);
                    }
				}

			}
		}


	}
}
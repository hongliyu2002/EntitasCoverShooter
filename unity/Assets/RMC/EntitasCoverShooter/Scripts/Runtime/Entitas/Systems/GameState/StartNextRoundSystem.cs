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
	public class StartNextRoundSystem : IReactiveSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_pool = pool;
		}

		public TriggerOnEvent trigger
        {
            get { return Matcher.StartNextRound.OnEntityAdded(); }
        }

        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
			{
				Entity entityBall = _pool.CreateEntity ();
                entityBall.AddPosition (new RMC.Common.UnityEngineReplacement.Vector3 (0,0,0));

                //Friction added in the y only

                entityBall.AddFriction      (GameConstants.BallFriction);
				entityBall.AddResource      ("Prefabs/Ball");
				entityBall.AddGoal          (1);
				entityBall.AddBoundsBounce  (-1);
                entityBall.AddTick          (0);

                //  The Entity holding the StartNextRound has been processed, so destroy the related Entity
                entity.WillDestroy(true);
                CoroutineUtility.Instance.StartCoroutineAfterDelay(StartNextRound_Coroutine(entityBall), 0.5f);
			}
		}

        /// <summary>
        /// Add a delay AFTER creating the ball visuall and BEFORE moving it. A courtesy to player.
        /// </summary>
        private IEnumerator StartNextRound_Coroutine (Entity entityBall)
        {
             entityBall.AddVelocity (GameConstants.GetBallInitialVelocity());
            yield return null;
        }

    }
}
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
    public class StartNextRoundSystem : ISetPool, IReactiveSystem
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<FooSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have these component(s)
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
                entityBall.AddTick          (0);

                //  The Entity holding the StartNextRound has been processed, so destroy the related Entity
                entity.AddDestroyMe(0);
                Timer.Register (0.5f, () => StartNextRound_Coroutine(entityBall));
			}
		}

        /// <summary>
        /// Add a delay AFTER creating the ball visuall and BEFORE moving it. A courtesy to player.
        /// </summary>
        private void StartNextRound_Coroutine (Entity entityBall)
        {
             entityBall.AddVelocity (GameConstants.GetBallInitialVelocity());
        }

    }
}
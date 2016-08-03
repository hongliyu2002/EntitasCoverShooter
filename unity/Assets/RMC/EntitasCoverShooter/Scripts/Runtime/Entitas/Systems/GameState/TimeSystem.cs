using Entitas;
using RMC.Common.Entitas.Components;
using System;

namespace RMC.Common.Entitas.Systems.GameState
{
	/// <summary>
	/// The TimeSystem always executes (even when the game is paused)
	/// It serves as an example of a system that will always execute, yet PARTIALLY respects isPaused
	/// </summary>
    public class TimeSystem : ISetPool, IInitializeSystem, IExecuteSystem
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Entity _gameEntity;
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<FooSystem>();
		public void SetPool(Pool pool) 
		{
			_pool = pool;
		}

        public void Initialize()
        {
            //By design: Systems created before Entities, so wait :)
            _pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Time)).OnEntityAdded += OnGameEntityAdded;
        }

		private void OnGameEntityAdded (Group group, Entity entity, int index, IComponent component)
		{
			_gameEntity = group.GetSingleEntity();
		}

		public void Execute() 
		{
            //always increment
            float timeSinceGameStartTotal = _gameEntity.time.timeSinceGameStartTotal + _gameEntity.tick.deltaTime;


            //sometimes increment
            if (!_gameEntity.time.isPaused)
            {
                //unpaused
                _gameEntity.ReplaceTime 
                (
                    _gameEntity.time.timeSinceGameStartUnpaused + _gameEntity.tick.deltaTime,
                    timeSinceGameStartTotal,
                    _gameEntity.time.isPaused
                );
            }
            else
            {
                _gameEntity.ReplaceTime 
                (
                    _gameEntity.time.timeSinceGameStartUnpaused,
                    timeSinceGameStartTotal,
                    _gameEntity.time.isPaused
                );
            }

			//Keep
			//Debug.LogFormat ("u:{0} t:{1}", _gameEntity.time.timeSinceGameStartUnpaused,  _gameEntity.time.timeSinceGameStartTotal );
	
		}

	}
}


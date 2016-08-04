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
    public class AISystem : ISetPool, IInitializeSystem, IExecuteSystem 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
        private Pool _pool;
		private Group _aiGroup;
        private Entity _playerEntity;


		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<FooSystem>();
		public void SetPool(Pool pool) 
		{
            _pool = pool;
		}

        public void Initialize()
        {
            // Get the group of entities that have these component(s)
            _aiGroup = _pool.GetGroup(Matcher.AllOf(Matcher.AI, Matcher.Position));
            _pool.GetGroup(Matcher.Player).OnEntityAdded += PlayerGroup_OnEntityAdded;
        }

        private void PlayerGroup_OnEntityAdded (Group group, Entity entity, int index, IComponent component)
        {
            _playerEntity = group.GetSingleEntity();
        }

		public void Execute() 
		{
            foreach (var aiEntity in _aiGroup.GetEntities())
            {
                System.Random r = new System.Random();
                if (r.Next(50) == 1)
                {
                    UnityEngine.Debug.Log("-");
                    UnityEngine.Debug.Log(aiEntity.position.position);
                    UnityEngine.Debug.Log(_playerEntity.position.position);
                    _pool.CreateEnemyBullet(
                        aiEntity.position.position + GameConstants.PositionOffsetBulletY + new Vector3 (0,0,1),
                        _playerEntity.position.position + GameConstants.PositionOffsetBulletY, 
                        GameConstants.BulletSpeed);
                }
            }
		}


	}
}
using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Systems.Transform
{
    /// <summary>
    /// Update PositionComponent via VelocityComponent
    /// </summary>
    public class VelocitySystem : ISetPool, IInitializeSystem, IExecuteSystem
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields
        private Pool _pool;
        private Group _group;

        // ------------------ Methods

        // Implement ISetPool to get the pool used when calling
        // pool.CreateSystem<VelocitySystem>();
        public void SetPool(Pool pool) 
        {
            _pool = pool;
        }

        public void Initialize()
        {
            // Get the group of entities that have these component(s)
            _group = _pool.GetGroup(Matcher.AllOf(Matcher.Velocity, Matcher.Position, Matcher.Friction, Matcher.Tick));
        }


        public void Execute() 
        {
            //Debug.Log ("VelocitySystem.Execute(), _group.count : " + _group.count);

            foreach (var entity in _group.GetEntities()) 
            {
                Vector3 velocity = new Vector3 (
                    entity.velocity.velocity.x,
                    entity.velocity.velocity.y,
                    entity.velocity.velocity.z
                );

                Vector3 position = new Vector3 (
                    entity.position.position.x,
                    entity.position.position.y,
                    entity.position.position.z
                );

                entity.ReplacePosition(new RMC.Common.UnityEngineReplacement.Vector3 (
                    (position.x + velocity.x * entity.tick.deltaTime) * (1- entity.friction.friction.x), 
                    (position.y + velocity.y * entity.tick.deltaTime) * (1- entity.friction.friction.y), 
                    (position.z + velocity.z * entity.tick.deltaTime) * (1- entity.friction.friction.z)
                ));


            }
        }


    }
}
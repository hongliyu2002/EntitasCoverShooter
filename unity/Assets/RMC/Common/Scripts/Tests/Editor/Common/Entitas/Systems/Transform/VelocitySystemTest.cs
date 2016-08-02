using NUnit.Framework;
using Entitas;
using RMC.Common.Entitas.Systems.Transform;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Transform
{
    /// <summary>
    /// We are testing my custom VelocitySystem. 
    /// This test is important because it is unique to my project (and thus not covered by Entitas' owners testing)
    /// Because Entitas Systems are decoupled from UnityEngine (typically), testing can be easy and thorough. Nice! - srivello
    /// </summary>

    [TestFixture]
    public class VelocitySystemTest
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields
        private Entity _testEnity;

        // ------------------ Methods

        [SetUp] 
        public void SetUp()
        {
            Pools.pool.Reset();
        }

        [TearDown] 
        public void TearDown()
        {
            Pools.pool.DestroyEntity(_testEnity);
            _testEnity = null;
            Pools.pool.Reset();
        }


        [Test]
        public void ExecuteSystemTest ([NUnit.Framework.Range (1, 10, 1)] int totalSystemExecutions)
        {
            //  Setup
            Vector3 velocity = new Vector3(10, 20, 30);
            Vector3 position = new Vector3(1, 2, 3);
            Vector3 expectedPosition = position;
            _testEnity = Pools.pool.CreateEntity()
                    .AddPosition(position)
                    .AddVelocity(velocity);
            
            //Desired. Strong typing when CreateSystem<T> is called. 
            VelocitySystem velocitySystem = Pools.pool.CreateSystem<VelocitySystem>() as VelocitySystem;

            //  This will run the system exacly once.
            for (var ex = 0; ex < totalSystemExecutions; ex++)
            {
                //for every execution
                velocitySystem.Execute();

                //we expect it to move by one velocity unit
                expectedPosition += velocity;
            }


            //  Assert
            Assert.AreEqual(expectedPosition, _testEnity.position.position, "The entity position will the original position plus one velocity.");
        }

    }

}

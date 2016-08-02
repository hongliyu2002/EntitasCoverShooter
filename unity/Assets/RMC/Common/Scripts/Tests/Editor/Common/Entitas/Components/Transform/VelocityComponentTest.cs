using NUnit.Framework;
using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Transform
{
    /// <summary>
    /// We are testing my custom VelocityComponent, but only testing really the 
    /// Entitas library itself. We can assume Entitas's owners test themselves,
    /// and thus this test is unecessary, but it is included as an example for learning. - srivello
    /// </summary>
    [TestFixture]
    public class VelocityComponentTest
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
        public void AddComponentTest ()
        {
            //  Setup
            _testEnity = Pools.pool.CreateEntity().AddVelocity(new Vector3(1, 2, 3));

            //  Assert
            Assert.AreEqual(new Vector3(1, 2, 3), _testEnity.velocity.velocity, "The entity velocity will be the same as previously 'added'.");
        }

        [Test]
        public void ReplaceComponentTest ()
        {
            //  Setup
            _testEnity = Pools.pool.CreateEntity().AddVelocity(new Vector3(1, 2, 3));
            _testEnity = Pools.pool.CreateEntity().ReplaceVelocity(new Vector3(11, 22, 33));

            //  Assert
            Assert.AreEqual(new Vector3(11, 22, 33), _testEnity.velocity.velocity, "The entity velocity will be the same as previously 'replaced'.");
        }


    }

}

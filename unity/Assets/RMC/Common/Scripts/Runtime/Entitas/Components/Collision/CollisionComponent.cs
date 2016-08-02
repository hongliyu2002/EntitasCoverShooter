using Entitas;

namespace RMC.Common.Entitas.Components.Collision
{
	/// <summary>
	/// Sent like an event: When collisions occur
	/// </summary>
	public class CollisionComponent : IComponent
	{
		public enum CollisionType
		{
			TriggerEnter,
			TriggerStay,
			TriggerExit

		}
		// ------------------ Serialized fields and properties
		public object localGameObject;
		public object otherGameObject;
		public CollisionType collisionType;


	}
}
using Entitas;

namespace RMC.EntitasCoverShooter.Entitas.Components
{
	/// <summary>
	/// Flags Entity to bounce off top/bottom of screen.
	/// </summary>
	public class BoundsBounceComponent : IComponent
	{
		// ------------------ Serialized fields and properties

        //negative required, "-1" means bounce at same speed as you entered. Default.
		public float bounceAmount = -1;		
	}
}
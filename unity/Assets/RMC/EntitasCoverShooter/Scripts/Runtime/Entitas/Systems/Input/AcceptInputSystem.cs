using Entitas;
using RMC.Common.Entitas.Components.Input;
using RMC.Common.UnityEngineReplacement;

namespace RMC.EntitasCoverShooter.Entitas.Systems
{
	/// <summary>
	/// Process input
	/// </summary>
	public class AcceptInputSystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _inputGroup;
        private Group _acceptInputGroup;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
            _inputGroup = pool.GetGroup(Matcher.AllOf(Matcher.Input));
            _acceptInputGroup = pool.GetGroup(Matcher.AllOf(Matcher.AcceptInput));

		}

		public void Execute() 
		{
			
            foreach (var inputEntity in _inputGroup.GetEntities()) 
            {

                //We choose to listen to axis, not buttons. But either is possible - srivello
                if (inputEntity.input.inputType == InputComponent.InputType.Axis)
                {
                    foreach (var acceptInputEntity in _acceptInputGroup.GetEntities())
                    {
                        //Debug.Log ("inputEntity.input.inputAxis.y : " + inputEntity.input.inputAxis.y);
                        Vector3 nextVelocity = new Vector3(0, inputEntity.input.inputAxis.y * 50, 0);
                        acceptInputEntity.ReplaceVelocity(nextVelocity);
                    }
                }

                //  The Entity holding the AcceptInputComponent has been processed, so destroy the related Entity
                inputEntity.WillDestroy(true);
            }

		}


	}
}
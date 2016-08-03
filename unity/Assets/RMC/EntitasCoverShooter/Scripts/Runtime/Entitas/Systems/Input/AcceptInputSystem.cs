using Entitas;
using RMC.Common.Entitas.Components.Input;
using RMC.Common.UnityEngineReplacement;
using RMC.EntitasCoverShooter.Entitas.Controllers;
using DG.Tweening;

namespace RMC.EntitasCoverShooter.Entitas.Systems
{
	/// <summary>
	/// Process input
	/// </summary>
    public class AcceptInputSystem : ISetPool, IInitializeSystem, IExecuteSystem
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
        private Pool _pool;
		private Group _inputGroup;
        private Group _acceptInputGroup;

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
            _inputGroup = _pool.GetGroup(Matcher.AllOf(Matcher.Input));
            _acceptInputGroup = _pool.GetGroup(Matcher.AllOf(Matcher.AcceptInput));

        }

		public void Execute() 
		{
			
            foreach (var inputEntity in _inputGroup.GetEntities()) 
            {
                if (inputEntity.input.inputKeyCode == KeyCode.Space)
                {
                    if (inputEntity.input.inputType == InputComponent.InputType.KeyDown)
                    {

                        Vector3 position;
                        foreach (var acceptInputEntity in _acceptInputGroup.GetEntities())
                        {
                            position = new Vector3(
                                acceptInputEntity.position.position.x,
                                0,
                                acceptInputEntity.position.position.z);
                            
                            acceptInputEntity.ReplacePosition(position, acceptInputEntity.position.useTween);
                        }
                    }
                    else if (inputEntity.input.inputType == InputComponent.InputType.KeyUp)
                    {
                        Vector3 position;
                        foreach (var acceptInputEntity in _acceptInputGroup.GetEntities())
                        {
                            position = new Vector3(
                                acceptInputEntity.position.position.x,
                                - 1,
                                acceptInputEntity.position.position.z);
                            
                            acceptInputEntity.ReplacePosition(position, acceptInputEntity.position.useTween);
                        }
                    }
                }
                else if (inputEntity.input.inputType == InputComponent.InputType.PointerDown)
                {
                    if (inputEntity.input.inputPointerPosition.y > CanvasController.Instance.StandButtonTopY)
                    {
                        //TODO: add a gun model and shoot from the barrel position
                        Entity playerEntity = _pool.GetGroup(Matcher.AllOf(Matcher.Player, Matcher.Position)).GetSingleEntity() ;
                        Entity enemyEntity = _pool.GetGroup(Matcher.AllOf(Matcher.Enemy, Matcher.Position)).GetSingleEntity();

                        Vector3 fromPosition = playerEntity.position.position + Vector3.up * 2;
                        Vector3 toPosition = enemyEntity.position.position + Vector3.up * 2;


                        //KEEP
                        //UnityEngine.Debug.Log ("Shoot from : " + fromPosition  + " + to " + toPosition);


                        _pool.CreateEntity().AddCreateBullet(
                                fromPosition, 
                                toPosition, 
                                GameConstants.BulletSpeed);
                    }
                }

                //  The Entity holding the AcceptInputComponent has been processed, so destroy the related Entity
                inputEntity.AddDestroyMe(0);
            }

		}


	}
}
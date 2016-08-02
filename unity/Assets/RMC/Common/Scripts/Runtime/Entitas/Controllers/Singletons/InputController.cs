using RMC.Common.Singleton;
using Entitas;
using UnityEngine;
using System.Collections.Generic;
using RMC.Common.Entitas.Components.Input;


namespace RMC.Common.Entitas.Controllers.Singleton
{
	/// <summary>
	/// Converts Unity Input to related Entity's
	/// </summary>
    public class InputController : SingletonMonobehavior<InputController> 
	{
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        private Pool _pool;

        // ------------------ Methods

        // ------------------ Non-serialized fields

        // ------------------ Methods

        protected void Start()
        {
            _pool = Pools.pool;
            InputController.OnDestroying += InputController_OnDestroying;

        }

        protected void InputController_OnDestroying(InputController instance)
        {
            InputController.OnDestroying -= InputController_OnDestroying;
        }

        protected void Update ()
        {
            float verticalAxis = Input.GetAxis("Vertical");
            float horizontalAxis = Input.GetAxis("Horizontal");

            if (verticalAxis != 0 || horizontalAxis != 0)
            {
                _pool.CreateEntity().AddInput(InputComponent.InputType.Axis, RMC.Common.UnityEngineReplacement.KeyCode.None, new RMC.Common.UnityEngineReplacement.Vector2(horizontalAxis, verticalAxis));
            }

            //TODO: Add button support
            //_pool.CreateEntity().AddInput (InputComponent.InputType.KeyCode, KeyCode.Return, Vector2.zero);

        }

    }


}

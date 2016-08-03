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


        // ------------------ Methods

        // ------------------ Non-serialized fields
        private Pool _pool;
        private float _verticalAxis;
        private float _horizontalAxis;
        private RMC.Common.UnityEngineReplacement.Vector2 zeroVector2 = RMC.Common.UnityEngineReplacement.Vector2.zero;
        Dictionary<KeyCode, RMC.Common.UnityEngineReplacement.KeyCode> _keyCodeDictionary;

        // ------------------ Methods

        protected void Start()
        {
            _pool = Pools.pool;
            InputController.OnDestroying += InputController_OnDestroying;
            SetupDictionary();

        }

        protected void InputController_OnDestroying(InputController instance)
        {
            InputController.OnDestroying -= InputController_OnDestroying;
        }

        protected void Update ()
        {
    
            CheckAxis();
            CheckMouse();
            CheckDictionary();

        }

        //TODO: Add MORE button support. Loop through a dictionary and call these 3 concepts?
        private void SetupDictionary()
        {

            _keyCodeDictionary = new Dictionary<KeyCode, RMC.Common.UnityEngineReplacement.KeyCode>();
            _keyCodeDictionary.Add(KeyCode.Space, RMC.Common.UnityEngineReplacement.KeyCode.Space);

        }

        private void CheckAxis()
        {
            _verticalAxis = Input.GetAxis("Vertical");
            _horizontalAxis = Input.GetAxis("Horizontal");
            if (_verticalAxis != 0 || _horizontalAxis != 0)
            {
                _pool.CreateEntity().AddInput(InputComponent.InputType.Axis, RMC.Common.UnityEngineReplacement.KeyCode.None, new RMC.Common.UnityEngineReplacement.Vector2(_horizontalAxis, _verticalAxis));
            }
        }

        private void CheckMouse()
        {

            if (Input.GetMouseButtonDown(0))
            {
                _pool.CreateEntity().AddInput(InputComponent.InputType.MouseButtonDown, RMC.Common.UnityEngineReplacement.KeyCode.None, zeroVector2 );

            }
            else if (Input.GetMouseButton(0))
            {
                _pool.CreateEntity().AddInput(InputComponent.InputType.MouseButtonDuring, RMC.Common.UnityEngineReplacement.KeyCode.None, zeroVector2);

            }
            else if (Input.GetMouseButtonDown(0))
            {
                _pool.CreateEntity().AddInput(InputComponent.InputType.MouseButtonUp, RMC.Common.UnityEngineReplacement.KeyCode.None, zeroVector2);

            }

        }

        private void CheckDictionary()
        {

            foreach (KeyValuePair<KeyCode, RMC.Common.UnityEngineReplacement.KeyCode> keyValuePair in _keyCodeDictionary)
            {
                if (Input.GetKeyDown(keyValuePair.Key))
                {
                    _pool.CreateEntity().AddInput (InputComponent.InputType.KeyCodeDown, keyValuePair.Value, zeroVector2);
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    _pool.CreateEntity().AddInput (InputComponent.InputType.KeyCodeDuring, keyValuePair.Value, zeroVector2);
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    _pool.CreateEntity().AddInput (InputComponent.InputType.KeyCodeUp, keyValuePair.Value, zeroVector2);
                }
            }

        }

    }


}

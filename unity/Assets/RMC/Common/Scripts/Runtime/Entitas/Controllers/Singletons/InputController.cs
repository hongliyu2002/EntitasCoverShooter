using RMC.Common.Singleton;
using Entitas;
using UnityEngine;
using System.Collections.Generic;
using RMC.Common.Entitas.Components.Input;
using RMC.Common.Entitas.Utilities;


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
        private RMC.Common.UnityEngineReplacement.Vector2 _pointerPosition;
        private RMC.Common.UnityEngineReplacement.Vector2 _zeroVector2 = RMC.Common.UnityEngineReplacement.Vector2.zero;
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
                _pool.CreateEntity().AddInput(InputComponent.InputType.Axis, RMC.Common.UnityEngineReplacement.KeyCode.None, new RMC.Common.UnityEngineReplacement.Vector2(_horizontalAxis, _verticalAxis), _zeroVector2);
            }
        }

        private void CheckMouse()
        {


            if (Input.GetMouseButtonDown(0))
            {
                _pointerPosition = new RMC.Common.UnityEngineReplacement.Vector2 (Input.mousePosition.x, Input.mousePosition.y);
                _pool.CreateEntity().AddInput(InputComponent.InputType.PointerDown, RMC.Common.UnityEngineReplacement.KeyCode.None, _zeroVector2, _pointerPosition );

            }
            else if (Input.GetMouseButton(0))
            {
                _pointerPosition = new RMC.Common.UnityEngineReplacement.Vector2 (Input.mousePosition.x, Input.mousePosition.y);
                _pool.CreateEntity().AddInput(InputComponent.InputType.PointerDuring, RMC.Common.UnityEngineReplacement.KeyCode.None, _zeroVector2, _pointerPosition);

            }
            else if (Input.GetMouseButtonDown(0))
            {
                _pointerPosition = new RMC.Common.UnityEngineReplacement.Vector2 (Input.mousePosition.x, Input.mousePosition.y);
                _pool.CreateEntity().AddInput(InputComponent.InputType.PointerUp, RMC.Common.UnityEngineReplacement.KeyCode.None, _zeroVector2, _pointerPosition);

            }

        }

        private void CheckDictionary()
        {

            foreach (KeyValuePair<KeyCode, RMC.Common.UnityEngineReplacement.KeyCode> keyValuePair in _keyCodeDictionary)
            {
                if (Input.GetKeyDown(keyValuePair.Key))
                {
                    _pool.CreateEntity().AddInput (InputComponent.InputType.KeyDown, keyValuePair.Value, _zeroVector2, _pointerPosition);
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    _pool.CreateEntity().AddInput (InputComponent.InputType.KeyDuring, keyValuePair.Value, _zeroVector2, _pointerPosition);
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    _pool.CreateEntity().AddInput (InputComponent.InputType.KeyUp, keyValuePair.Value, _zeroVector2, _pointerPosition);
                }
            }

        }

    }


}

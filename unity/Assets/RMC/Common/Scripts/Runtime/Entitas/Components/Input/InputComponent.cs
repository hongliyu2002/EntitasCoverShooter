﻿using Entitas;
using RMC.Common.UnityEngineReplacement;

namespace RMC.Common.Entitas.Components.Input
{
	/// <summary>
    /// Sent like an event: Indicating input has occurred
	/// </summary>
	public class InputComponent : IComponent
	{
        public enum InputType
        {
            Axis,

            KeyDown,
            KeyDuring,
            KeyUp,

            PointerDown,
            PointerDuring,
            PointerUp

        }
           
		// ------------------ Serialized fields and properties
        public InputType inputType;

        //The type dictates which ONE of these will be not-null
        public KeyCode inputKeyCode;
        public Vector2 inputAxis;
        public Vector2 inputPointerPosition;

	}
}
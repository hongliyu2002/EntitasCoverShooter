namespace RMC.Common.UnityEngineReplacement
{
   
    /// <summary>
    /// UnityEngineReplacement for: UnityEngine.Vector2
    /// </summary>
    public class Vector2
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        public float x;
        public float y;

        // ------------------ Non-serialized fields

        // ------------------ Methods
        public Vector2 (float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 ()
        {
            this.x = Vector2.zero.x;
            this.y = Vector2.zero.y;
        }

        public static Vector2 zero
        {
            get
            {
                return new Vector2 (0,0);
            }

        }

    }
}
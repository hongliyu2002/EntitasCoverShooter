namespace RMC.Common.UnityEngineReplacement
{

    /// <summary>
    /// UnityEngineReplacement for: UnityEngine.Bounds
    /// </summary>
    public class Bounds
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        public Vector3 center
        {
            get;
            set;
        }
            

        public Vector3 max
        {
            get
            {
                return center + size/2;
            }
        }

        public Vector3 min
        {
            get
            {
                return center - size/2;
            }
        }

        public Vector3 size
        {
            get;
            set;
        }

        // ------------------ Non-serialized fields

        // ------------------ Methods
        public Bounds (Vector3 center, Vector3 size)
        {
            this.center = center;
            this.size = size;
        }

        public Bounds ()
        {
            this.center = Vector3.zero;
            this.size = Vector3.zero;
        }



    }
}

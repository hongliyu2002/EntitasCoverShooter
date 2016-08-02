namespace RMC.Common.UnityEngineReplacement
{
   
    /// <summary>
    /// UnityEngineReplacement for: UnityEngine.Vector3
    /// </summary>
    public class Vector3
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        public float x;
        public float y;
        public float z;

        // ------------------ Non-serialized fields

        // ------------------ Methods
        public Vector3 (float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ()
        {
            this.x = Vector3.zero.x;
            this.y = Vector3.zero.y;
            this.z = Vector3.zero.z;
        }

        public static Vector3 zero
        {
            get
            {
                return new Vector3 (0,0,0);
            }

        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2) 
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2) 
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator /(Vector3 v1, float d) 
        {
            return new Vector3(v1.x/d, v1.y/d, v1.z/d);
        }
        public static Vector3 operator *(Vector3 v1, float m) 
        {
            return new Vector3(v1.x*m, v1.y*m, v1.z*m);
        }

        public override string ToString()
        {
            return string.Format("[Vector3 ({0},{1},{2}]", x, y, z);
        }

    }
}
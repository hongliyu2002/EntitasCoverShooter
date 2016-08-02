using UnityEngine;

namespace RMC.Common.Utilities
{
    /// <summary>
    /// Some potentially reusable utilities
    /// Can be separated intos more specific classes later - srivello
    /// </summary>
    public class GameUtility
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields

        // ------------------ Methods

        /// <summary>
        /// Gives the 3D coordinates for the edge of the camera at a z of 0
        /// </summary>
        public static Bounds GetOrthographicBounds(Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds
            (
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0)
            );
            return bounds;
        }


        /// <summary>
        /// Gets a random within range with equal odds of being positive or negative
        /// </summary>
        public static float GetRandomRangePosNeg (float positiveMin, float positiveMax)
        {
            //start with only positive
            float randomResult = Random.Range(Mathf.Abs (positiveMin), Mathf.Abs(positiveMax));
            if (Random.value > 0.5)
            {
                randomResult *= -1;   
            }
            return randomResult;

        }


    }


}

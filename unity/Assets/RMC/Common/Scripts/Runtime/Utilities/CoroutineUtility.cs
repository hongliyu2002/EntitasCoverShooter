using RMC.Common.Singleton;
using System.Collections;
using UnityEngine;

namespace RMC.Common.Utilities
{
    /// <summary>
    /// Allow non-Monobehaviors to call Coroutines
    /// </summary>
    public class CoroutineUtility : SingletonMonobehavior<CoroutineUtility> 
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields

        // ------------------ Methods

        new public Coroutine StartCoroutine (IEnumerator iEnumerator)
        {
            if (iEnumerator != null)
            {
                return base.StartCoroutine(iEnumerator);
            }
            else
            {
                return null;
            }

        }


    }


}
